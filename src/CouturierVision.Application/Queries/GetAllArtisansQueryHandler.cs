using MediatR;
using CouturierVision.Application.DTOs;
using CouturierVision.Domain.Interfaces;

namespace CouturierVision.Application.Queries;

public class GetAllArtisansQueryHandler : IRequestHandler<GetAllArtisansQuery, IReadOnlyList<ArtisanDto>>
{
    private readonly IArtisanRepository _artisanRepository;

    public GetAllArtisansQueryHandler(IArtisanRepository artisanRepository)
    {
        _artisanRepository = artisanRepository;
    }

    public async Task<IReadOnlyList<ArtisanDto>> Handle(GetAllArtisansQuery request, CancellationToken cancellationToken)
    {
        var artisans = await _artisanRepository.GetAllAsync(cancellationToken);
        return artisans.Select(a => new ArtisanDto(
            a.Id,
            a.FirstName,
            a.LastName,
            a.Email.Value,
            a.PhoneNumber,
            a.Specialization,
            a.CreatedAt)).ToList();
    }
}
