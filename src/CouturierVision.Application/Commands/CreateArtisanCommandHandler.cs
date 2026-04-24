using MediatR;
using CouturierVision.Application.DTOs;
using CouturierVision.Domain.Entities;
using CouturierVision.Domain.Interfaces;
using CouturierVision.Domain.ValueObjects;

namespace CouturierVision.Application.Commands;

public class CreateArtisanCommandHandler : IRequestHandler<CreateArtisanCommand, ArtisanDto>
{
    private readonly IArtisanRepository _artisanRepository;

    public CreateArtisanCommandHandler(IArtisanRepository artisanRepository)
    {
        _artisanRepository = artisanRepository;
    }

    public async Task<ArtisanDto> Handle(CreateArtisanCommand request, CancellationToken cancellationToken)
    {
        var artisan = new Artisan(
            Guid.NewGuid(),
            request.FirstName,
            request.LastName,
            new Email(request.Email),
            request.PhoneNumber,
            request.Specialization);

        await _artisanRepository.AddAsync(artisan, cancellationToken);

        return new ArtisanDto(
            artisan.Id,
            artisan.FirstName,
            artisan.LastName,
            artisan.Email.Value,
            artisan.PhoneNumber,
            artisan.Specialization,
            artisan.CreatedAt);
    }
}
