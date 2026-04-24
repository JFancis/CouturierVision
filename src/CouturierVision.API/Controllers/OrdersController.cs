using MediatR;
using Microsoft.AspNetCore.Mvc;
using CouturierVision.Application.Commands;
using CouturierVision.Application.DTOs;

namespace CouturierVision.API.Controllers;

/// <summary>Orders management</summary>
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

    /// <summary>Create a new order (Draft status)</summary>
    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(Advance), new { id = result.Id }, result);
    }

    /// <summary>Advance order to next status</summary>
    [HttpPut("{id:guid}/advance")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Advance(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new AdvanceOrderStatusCommand(id), ct);
        return Ok(result);
    }

    /// <summary>Register a deposit payment for an order</summary>
    [HttpPost("{id:guid}/deposit")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegisterDeposit(Guid id, [FromBody] RegisterDepositRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new RegisterDepositCommand(id, request.Amount), ct);
        return Ok(result);
    }
}

public record RegisterDepositRequest(decimal Amount);
