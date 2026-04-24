using CouturierVision.Application.Commands;
using CouturierVision.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CouturierVision.API.Controllers;

/// <summary>
/// Manages orders and production workflow.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Creates a new order (Draft status).</summary>
    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>Advances the order to the next production status.</summary>
    [HttpPut("{id:guid}/advance")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AdvanceStatus(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new AdvanceOrderStatusCommand(id), ct);
        return NoContent();
    }

    /// <summary>Registers a deposit payment on the order.</summary>
    [HttpPost("{id:guid}/deposit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegisterDeposit(Guid id, [FromBody] RegisterDepositRequest request, CancellationToken ct)
    {
        await _mediator.Send(new RegisterDepositCommand(id, request.Amount), ct);
        return NoContent();
    }
}

public record RegisterDepositRequest(decimal Amount);
