using System.ComponentModel;

namespace Venice.Orders.Domain.Enums;

public enum ExceptionsMessages
{
    [Description("Invalid request.")]
    InvalidRequest,

    [Description("Invalid input data: {PropertyValue}.")]
    InvalidInputData,

    [Description("Invalid API Key credentials for the requested resource.")]
    Unauthorized,

    [Description("Permission denied to access this resource.")]
    Forbidden,

    [Description("Resource not found.")]
    NotFound,
    
    [Description("The server was unable to process the request.")]
    BadRequest,

    [Description("The server was unable to process the request.")]
    InternalServerError,

    [Description("The server was unable to process the request.")]
    ArgumentNull,

    [Description("The server was unable to process the request.")]
    DatabaseException,
    
    [Description("A record with these values already exists.")]
    Conflict,
}