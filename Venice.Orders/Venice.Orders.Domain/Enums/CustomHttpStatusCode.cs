namespace Venice.Orders.Domain.Enums;

public enum CustomHttpStatusCode
{
    // Successful (2xx)
    OK = 200,
    Created = 201,
    Accepted = 202,
    NoContent = 204,

    // Client Errors (4xx)
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    Conflict = 409,
    UnsupportedMediaType = 415,
    TooManyRequests = 429,
    // Custom Extension
    ClientClosedRequest = 499,

    // Server Errors (5xx)
    InternalServerError = 500,
    NotImplemented = 501,
    ServiceUnavailable = 503,
}