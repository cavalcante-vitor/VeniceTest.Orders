using Microsoft.Extensions.Logging;
using Moq;

namespace Venice.Orders.UnitTests.Utils;

public abstract class TestBase
{
    protected Mock<ILogger<T>> CreateLoggerMock<T>() => new();
    
    protected void VerifyLogCalled<T>(Mock<ILogger<T>> loggerMock, LogLevel level, string message)
    {
        loggerMock.Verify(
            x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(message)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
