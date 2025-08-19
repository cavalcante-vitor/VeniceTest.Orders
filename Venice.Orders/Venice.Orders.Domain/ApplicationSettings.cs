using System.Diagnostics.CodeAnalysis;

namespace Venice.Orders.Domain;

[ExcludeFromCodeCoverage]
public static class ApplicationSettings
{
    public static string MicroserviceName => "venice.orders";
    
    public static string DateFormat => "yyyy-MM-dd HH:mm:ss";

    public static int MaxRecords => 100;
    
    public static string ErrorResponseContentType => "application/json";
    
    public static string CorsPolicyName => "CorsPolicy";
}