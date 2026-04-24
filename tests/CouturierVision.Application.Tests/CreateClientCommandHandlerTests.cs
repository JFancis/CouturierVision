using CouturierVision.Application.Commands;
using CouturierVision.Domain.Entities;
using CouturierVision.Domain.Interfaces;
using Moq;
using Xunit;

namespace CouturierVision.Application.Tests;

public class CreateClientCommandHandlerTests
{
    private readonly Mock<IClientRepository> _repoMock = new();
    private readonly CreateClientCommandHandler _handler;

    public CreateClientCommandHandlerTests()
    {
        _handler = new CreateClientCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsClientDto()
    {
        var command = new CreateClientCommand("Jean", "Dupont", "jean@example.com", "+33612345678", "Classic");
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("jean@example.com", result.Email);
        Assert.Equal("Jean", result.FirstName);
        Assert.Equal("Dupont", result.LastName);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
