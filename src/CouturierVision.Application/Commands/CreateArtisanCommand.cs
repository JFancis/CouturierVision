using MediatR;
using CouturierVision.Application.DTOs;

namespace CouturierVision.Application.Commands;

public record CreateArtisanCommand(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Specialization) : IRequest<ArtisanDto>;
