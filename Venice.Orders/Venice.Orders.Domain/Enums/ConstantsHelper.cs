using System.Diagnostics.CodeAnalysis;

namespace Venice.Orders.Domain.Enums;

[ExcludeFromCodeCoverage]
public static class ConstantsHelper
{
    public const string DateFormat = "yyyy-MM-dd HH:mm:ss";

    public const int MaxRecords = 100;
    
    public const string ErrorResponseContentType = "application/json";
    
    public const string CorsPolicyName = "CorsPolicy";
    
    public const string DefaultConnectionDatabase = "Default";
}