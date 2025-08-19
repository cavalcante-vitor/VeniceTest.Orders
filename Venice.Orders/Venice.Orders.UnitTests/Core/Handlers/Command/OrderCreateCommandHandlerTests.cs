using FluentAssertions;
using Moq;
using Venice.Orders.Core.Handlers.Commands;
using Venice.Orders.Core.Models;
using Venice.Orders.Domain.Entities;
using Venice.Orders.Domain.Repositories;
using Venice.Orders.Domain.Services;
using Venice.Orders.Infrastructure.DataProviders.Contexts;
using Venice.Orders.UnitTests.Utils;
using Venice.Orders.UnitTests.Utils.Builders;

namespace Venice.Orders.UnitTests.Core.Handlers.Command;

public class OrderCreateCommandHandlerTests : TestBase, IDisposable
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IMongoRepository<OrderItem>> _orderItemRepositoryMock;
    private readonly Mock<IEventBus> _eventBusMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly OrderCreateCommandHandler _handler;

    public OrderCreateCommandHandlerTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _orderItemRepositoryMock = new Mock<IMongoRepository<OrderItem>>();
        _eventBusMock = new Mock<IEventBus>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new OrderCreateCommandHandler(
            _orderRepositoryMock.Object,
            _orderItemRepositoryMock.Object,
            _eventBusMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldCreateOrderSuccessfully()
    {
        // Arrange
        var request = OrderCreateRequestBuilder.Create().Build();
        
        _unitOfWorkMock
            .Setup(x => x.ExecuteTransactionAsync(It.IsAny<Func<Task>>(), It.IsAny<CancellationToken>()))
            .Callback<Func<Task>, CancellationToken>(async (func, _) => await func())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OrderQueryResponse>();
        
        _orderRepositoryMock.Verify(
            x => x.CreateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldSaveOrderItemsCorrectly()
    {
        // Arrange
        var request = OrderCreateRequestBuilder.Create()
            .WithItems(2)
            .Build();
        
        _unitOfWorkMock
            .Setup(x => x.ExecuteTransactionAsync(It.IsAny<Func<Task>>(), It.IsAny<CancellationToken>()))
            .Callback<Func<Task>, CancellationToken>(async (func, _) => await func())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _orderItemRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<OrderItem>()),
            Times.Exactly(2));
    }

  [Fact]
    public async Task Handle_ValidRequest_ShouldPublishOrderCreatedEvent()
    {
        // Arrange
        var request = OrderCreateRequestBuilder.Create().Build();
        
        _unitOfWorkMock
            .Setup(x => x.ExecuteTransactionAsync(It.IsAny<Func<Task>>(), It.IsAny<CancellationToken>()))
            .Callback<Func<Task>, CancellationToken>(async (func, _) => await func())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _eventBusMock.Verify(
            x => x.Publish(
                "venice.orders.created",
                It.IsAny<OrderCreatedEvent>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_TransactionThrowsException_ShouldPropagateException()
    {
        // Arrange
        var request = OrderCreateRequestBuilder.Create().Build();
        var expectedException = new InvalidOperationException("Database error");
        
        _unitOfWorkMock
            .Setup(x => x.ExecuteTransactionAsync(It.IsAny<Func<Task>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(request, CancellationToken.None));
        
        exception.Should().Be(expectedException);
        
        // Verify que NÃO tentou publicar evento se a transação falhou
        _eventBusMock.Verify(
            x => x.Publish(It.IsAny<string>(), It.IsAny<OrderCreatedEvent>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_EventBusThrowsException_ShouldStillCompleteTransactionFirst()
    {
        // Arrange
        var request = OrderCreateRequestBuilder.Create().Build();
        
        _unitOfWorkMock
            .Setup(x => x.ExecuteTransactionAsync(It.IsAny<Func<Task>>(), It.IsAny<CancellationToken>()))
            .Callback<Func<Task>, CancellationToken>(async (func, _) => await func())
            .Returns(Task.CompletedTask);
        
        _eventBusMock
            .Setup(x => x.Publish(It.IsAny<string>(), It.IsAny<OrderCreatedEvent>()))
            .ThrowsAsync(new InvalidOperationException("Event bus error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(request, CancellationToken.None));
        
        // Verify que executou a transação primeiro
        _unitOfWorkMock.Verify(
            x => x.ExecuteTransactionAsync(It.IsAny<Func<Task>>(), It.IsAny<CancellationToken>()),
            Times.Once);
        
        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_MultipleItems_ShouldAddAllItemsToOrder()
    {
        // Arrange
        var request = OrderCreateRequestBuilder.Create()
            .WithItems(3)
            .Build();
        
        Order? capturedOrder = null;
        _orderRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Callback<Order, CancellationToken>((order, _) => capturedOrder = order)
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.ExecuteTransactionAsync(It.IsAny<Func<Task>>(), It.IsAny<CancellationToken>()))
            .Callback<Func<Task>, CancellationToken>(async (func, _) => await func())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        capturedOrder.Should().NotBeNull();
        capturedOrder!.Items.Should().HaveCount(3);
        
        // Verifica se cada item foi adicionado corretamente
        var items = capturedOrder.Items.ToList();
        items[0].Quantity.Should().Be(1);
        items[1].Quantity.Should().Be(2);
        items[2].Quantity.Should().Be(3);
    }

    public void Dispose()
    {
    }
}
