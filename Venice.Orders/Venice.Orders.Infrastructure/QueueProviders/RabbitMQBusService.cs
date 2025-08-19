using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Venice.Orders.Domain.Entities;
using Venice.Orders.Domain.Services;

namespace Venice.Orders.Infrastructure.QueueProviders;

public class RabbitMqBusService(
    IConfiguration configuration,
    ILogger<RabbitMqBusService> logger)
    : IEventBus, IAsyncDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    private readonly ILogger<RabbitMqBusService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly HashSet<string> _declaredQueues = [];
    private readonly SemaphoreSlim _queueDeclarationSemaphore = new(1, 1);
    private readonly SemaphoreSlim _initializationSemaphore = new(1, 1);
    private bool _isInitialized = false;
    private bool _disposed = false;

    private async Task EnsureInitializedAsync()
    {
        if (_isInitialized && _connection?.IsOpen == true && _channel?.IsOpen == true)
            return;

        await _initializationSemaphore.WaitAsync();
        try
        {
            if (_isInitialized && _connection?.IsOpen == true && _channel?.IsOpen == true)
                return;

            await InitializeConnectionAsync();
            _isInitialized = true;
        }
        finally
        {
            _initializationSemaphore.Release();
        }
    }

    private async Task InitializeConnectionAsync()
    {
        try
        {
            if (_channel != null)
            {
                if (_channel.IsOpen)
                    await _channel.CloseAsync();
                _channel.Dispose();
            }

            if (_connection != null)
            {
                if (_connection.IsOpen)
                    await _connection.CloseAsync();
                _connection.Dispose();
            }

            var connectionString = _configuration["MessageBroker:Host"];
            var factory = new ConnectionFactory() { Uri = new Uri(connectionString) };
            
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            
            var exchange = _configuration["MessageBroker:Exchange"];
            await _channel.ExchangeDeclareAsync(
                exchange: exchange, 
                type: ExchangeType.Topic, 
                durable: true,
                autoDelete: false,
                arguments: null);

            _logger.LogInformation("RabbitMQ publisher initialized successfully. Exchange: {Exchange}", exchange);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize RabbitMQ publisher connection");
            throw;
        }
    }

    public async Task Publish<T>(string routingKey, T @event) where T : Event
    {
        await EnsureInitializedAsync();
        await EnsureQueueExistsAsync(routingKey);

        try
        {
            var exchange = _configuration["MessageBroker:Exchange"];
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));

            _logger.LogInformation("Publishing event to RabbitMQ. Exchange: {Exchange}, RoutingKey: {RoutingKey}, EventType: {EventType}", 
                exchange, routingKey, typeof(T).Name);

            var properties = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json",
                DeliveryMode = DeliveryModes.Persistent,
                MessageId = Guid.NewGuid().ToString(),
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds()),
                Headers = new Dictionary<string, object?>
                {
                    ["event-type"] = typeof(T).Name,
                    ["published-at"] = DateTimeOffset.UtcNow.ToString("O")
                }
            };

            await _channel!.BasicPublishAsync(
                exchange: exchange,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: properties,
                body: body,
                cancellationToken: CancellationToken.None);

            _logger.LogDebug("Event published successfully. RoutingKey: {RoutingKey}, MessageId: {MessageId}", 
                routingKey, properties.MessageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish event. RoutingKey: {RoutingKey}, EventType: {EventType}", 
                routingKey, typeof(T).Name);
            throw;
        }
    }

    private async Task EnsureQueueExistsAsync(string queueName)
    {
        if (_declaredQueues.Contains(queueName))
            return;

        await _queueDeclarationSemaphore.WaitAsync();
        try
        {
            if (_declaredQueues.Contains(queueName))
                return;

            var exchange = _configuration["MessageBroker:Exchange"];

            await _channel!.QueueDeclareAsync(
                queue: queueName,
                durable: true, 
                exclusive: false,
                autoDelete: false,
                arguments: null);

            await _channel.QueueBindAsync(
                queue: queueName,
                exchange: exchange,
                routingKey: queueName,
                arguments: null);

            _declaredQueues.Add(queueName);

            _logger.LogInformation("Queue {Queue} declared and bound to exchange {Exchange}", queueName, exchange);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to ensure queue {Queue} exists", queueName);
            throw;
        }
        finally
        {
            _queueDeclarationSemaphore.Release();
        }
    }

    
    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        try
        {
            _logger.LogInformation("Disposing RabbitMQ publisher resources");

            if (_channel != null)
            {
                if (_channel.IsOpen)
                    await _channel.CloseAsync(200, "Publisher shutdown");
                _channel.Dispose();
                _channel = null;
            }

            if (_connection != null)
            {
                if (_connection.IsOpen)
                    await _connection.CloseAsync(200, "Publisher shutdown");
                _connection.Dispose();
                _connection = null;
            }

            _initializationSemaphore.Dispose();
            _disposed = true;

            _logger.LogInformation("RabbitMQ publisher resources disposed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disposing RabbitMQ publisher resources");
        }
    }
}