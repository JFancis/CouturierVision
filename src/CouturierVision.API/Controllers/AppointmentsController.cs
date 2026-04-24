using MediatR;
using Microsoft.AspNetCore.Mvc;
using CouturierVision.Application.Commands;
using CouturierVision.Application.DTOs;
using CouturierVision.Application.Queries;

namespace CouturierVision.API.Controllers;

/// <summary>Appointments management</summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AppointmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Get appointments for a client</summary>
    [HttpGet("client/{clientId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<AppointmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByClient(Guid clientId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAppointmentsByClientQuery(clientId), ct);
        return Ok(result);
    }

    /// <summary>Create a new appointment</summary>
    [HttpPost]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetByClient), new { clientId = result.ClientId }, result);
    }
}
