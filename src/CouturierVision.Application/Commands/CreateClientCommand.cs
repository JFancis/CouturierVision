using CouturierVision.Application.DTOs;
using MediatR;

namespace CouturierVision.Application.Commands;

public record CreateClientCommand(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string StylePreferences = "") : IRequest<ClientDto>;
