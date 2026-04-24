using CouturierVision.Application.Commands;
using CouturierVision.Domain.Entities;
using CouturierVision.Domain.Interfaces;
using Moq;
using Xunit;

namespace CouturierVision.Application.Tests;

public class CreateClientCommandHandlerTests
{
    private readonly Mock<IClientRepository> _repositoryMock = new();

    [Fact]
    public async Task Handle_ValidCommand_ReturnsClientDto()
    {
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new CreateClientCommandHandler(_repositoryMock.Object);
        var command = new CreateClientCommand("Jean", "Dupont", "jean@example.com", "0601020304", "Classique");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Jean", result.FirstName);
        Assert.Equal("jean@example.com", result.Email);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidEmail_ThrowsDomainException()
    {
        var handler = new CreateClientCommandHandler(_repositoryMock.Object);
        var command = new CreateClientCommand("Jean", "Dupont", "not-an-email", "0601020304", "Classique");

        await Assert.ThrowsAsync<CouturierVision.Domain.Exceptions.DomainException>(
            () => handler.Handle(command, CancellationToken.None));
    }
}
