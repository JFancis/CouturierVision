using MediatR;
using CouturierVision.Application.DTOs;

namespace CouturierVision.Application.Queries;

public record GetAllArtisansQuery : IRequest<IReadOnlyList<ArtisanDto>>;
