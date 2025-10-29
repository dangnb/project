using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MNG.API.Abtractions;

namespace MNG.API.Controllers.V1;

[ApiVersion(1)]
public class AuthController : ApiController
{
    private readonly ILogger<AuthController> _logger;

    public AuthController(ISender sender, ILogger<AuthController> logger) : base(sender: sender)
    {
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpPost(Name = "login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Login([FromBody] Contract.Services.V1.Identity.Query.Login login)
    {
        var result = await Sender.Send(login);
        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }
        return Ok(result);
    }

}
