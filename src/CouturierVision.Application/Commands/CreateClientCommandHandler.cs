using CouturierVision.Application.DTOs;
using CouturierVision.Domain.Entities;
using CouturierVision.Domain.Interfaces;
using MediatR;

namespace CouturierVision.Application.Commands;

public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, ClientDto>
{
    private readonly IClientRepository _clientRepository;

    public CreateClientCommandHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<ClientDto> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var client = Client.Create(
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber,
            request.StylePreferences);

        await _clientRepository.AddAsync(client, cancellationToken);

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
