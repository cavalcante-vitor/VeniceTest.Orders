using MediatR;
using Microsoft.AspNetCore.Mvc;
using Venice.Orders.Core.Mappers;
using Venice.Orders.Core.Models;

namespace Venice.Orders.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthsController(IMediator mediator) : ControllerBase
{
    [HttpPost("token")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTokenAsync(
        [FromHeader(Name = "x-api-key")] string? apiKey,
        CancellationToken cancellationToken = default)
    {
        var request = AuthMapper.ToCreate(apiKey);
        var tokenResponse = await mediator.Send(request, cancellationToken);
        return Ok(tokenResponse);
    }
}