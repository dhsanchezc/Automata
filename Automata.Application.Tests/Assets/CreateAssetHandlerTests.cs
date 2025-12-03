using AutoMapper;
using Automata.Application.Assets.Commands;
using Automata.Application.Assets.Handlers;
using Automata.Domain.Aggregates.Assets;
using Automata.Domain.Common;
using Automata.Domain.Ports.Repositories;
using Moq;

namespace Automata.Application.Tests.Assets;

public class CreateAssetHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_AddAssetAndSaves()
    {
        // Arrange: create mock + Setup behavior
        var repo = new Mock<IAssetRepository>();
        var uow = new Mock<IUnitOfWork>();
        repo.SetupGet(r => r.UnitOfWork).Returns(uow.Object);

        var mapper = new Mock<IMapper>();
        var command = new CreateAssetCommand("Pump A", "Main line");
        var mapped = new Asset();

        mapper.Setup(m => m.Map<Asset>(command)).Returns(mapped);
        uow.Setup(x => x.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var sut = new CreateAssetHandler(repo.Object, mapper.Object);

        // Act
        await sut.Handle(command, CancellationToken.None);

        // Assert: Verify interactions
        mapper.Verify(m => m.Map<Asset>(command), Times.Once);
        repo.Verify(r => r.Add(mapped), Times.Once);
        uow.Verify(x => x.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}