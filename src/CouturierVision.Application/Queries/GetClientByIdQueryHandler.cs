using CouturierVision.Application.DTOs;
using CouturierVision.Domain.Interfaces;
using MediatR;

namespace CouturierVision.Application.Queries;

public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, ClientDto?>
{
    private readonly IClientRepository _clientRepository;

    public GetClientByIdQueryHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<ClientDto?> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(request.Id, cancellationToken);
        if (client is null) return null;

        return new ClientDto(
            client.Id,
            client.FirstName,
            client.LastName,
            client.Email.Value,
            client.PhoneNumber,
            client.StylePreferences,
            client.Measurements?.Json,
            client.CreatedAt);
    }
}
