using MediatR;
using CouturierVision.Application.DTOs;
using CouturierVision.Domain.Interfaces;

namespace CouturierVision.Application.Queries;

public class GetAllClientsQueryHandler : IRequestHandler<GetAllClientsQuery, IReadOnlyList<ClientDto>>
{
    private readonly IClientRepository _clientRepository;

    public GetAllClientsQueryHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<IReadOnlyList<ClientDto>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
    {
        var clients = await _clientRepository.GetAllAsync(cancellationToken);
        return clients.Select(c => new ClientDto(
            c.Id,
            c.FirstName,
            c.LastName,
            c.Email.Value,
            c.PhoneNumber,
            c.StylePreferences,
            c.Measurements?.Json,
            c.CreatedAt)).ToList();
    }
}
