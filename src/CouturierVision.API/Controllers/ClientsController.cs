using CouturierVision.Application.Commands;
using CouturierVision.Application.DTOs;
using CouturierVision.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CouturierVision.API.Controllers;

/// <summary>
/// Manages clients and their profiles.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ClientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Creates a new client.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Gets a client by ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetClientByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }
}
