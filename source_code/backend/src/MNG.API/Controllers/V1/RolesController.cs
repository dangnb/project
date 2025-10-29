using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MNG.API.Abtractions;
using MNG.Contract.Services.V1.Role;

namespace MNG.API.Controllers.V1;

[ApiVersion(1)]
public class RolesController : ApiController
{
    private readonly ILogger<RolesController> _logger;

    public RolesController(ISender sender, ILogger<RolesController> logger) : base(sender: sender)
    {
        _logger = logger;
    }

    [HttpPost(Name = "CreateRoles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateRoles([FromBody] Command.CreateRoleCommand createRole)
    {
        var result = await Sender.Send(createRole);
        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Ok(result);
    }
}
