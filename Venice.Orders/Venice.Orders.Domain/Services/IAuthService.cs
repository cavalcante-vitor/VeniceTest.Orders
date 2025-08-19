namespace Venice.Orders.Domain.Services;

public interface IAuthService
{
    public bool IsValidApiKey(string apiKey);

    public string GenerateToken(CancellationToken cancellationToken = default);
}
