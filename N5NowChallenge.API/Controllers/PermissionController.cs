using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5NowChallenge.Application.Commands.CreatePermission;
using N5NowChallenge.Application.Commands.UpdatePermission;
using N5NowChallenge.Application.DTOs;
using N5NowChallenge.Application.Queries.Base;
using N5NowChallenge.Application.Queries.GetPermissions;
using Nest;
using System.Net;

namespace N5NowChallenge.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PermissionController : Controller
{
    private readonly IMediator _mediator;
    public PermissionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    [ProducesResponseType(typeof(IEnumerable<PermissionDTO>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<GetAllPermissionResponse>> GetPermissions(CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetAllPermissionsQuery(), cancellationToken));
    }

    [HttpPost("")]
    [ProducesResponseType(typeof(PermissionDTO), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreatePermissionResponse>> RequestPermission([FromBody] CreatePermissionCommand command, CancellationToken cancellationToken)
    {
        return CreatedAtAction(nameof(GetPermissions), await _mediator.Send(command, cancellationToken));
    }

    [HttpPut("")]
    [ProducesResponseType(typeof(PermissionDTO), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<UpdatePermissionResponse>> ModifyPermission([FromBody] UpdatePermissionCommand command, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(command, cancellationToken));
    }
}
