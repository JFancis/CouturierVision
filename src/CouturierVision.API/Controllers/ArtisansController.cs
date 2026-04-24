using MediatR;
using Microsoft.AspNetCore.Mvc;
using CouturierVision.Application.Commands;
using CouturierVision.Application.DTOs;
using CouturierVision.Application.Queries;

namespace CouturierVision.API.Controllers;

/// <summary>Artisans management</summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ArtisansController : ControllerBase
{
    private readonly IMediator _mediator;

    public ArtisansController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Get all artisans</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ArtisanDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllArtisansQuery(), ct);
        return Ok(result);
    }

    /// <summary>Create a new artisan</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ArtisanDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateArtisanCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetAll), new { }, result);
    }

    /// <summary>Assign an artisan to an order</summary>
    [HttpPut("{artisanId:guid}/orders/{orderId:guid}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignToOrder(Guid artisanId, Guid orderId, CancellationToken ct)
    {
        var result = await _mediator.Send(new AssignArtisanToOrderCommand(orderId, artisanId), ct);
        return Ok(result);
    }
}
