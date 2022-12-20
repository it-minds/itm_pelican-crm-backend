using System.Linq.Expressions;
using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Clients.HubSpotCommands.DeleteClient;
using Pelican.Domain;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.Clients.Commands.DeleteClient;
public class DeleteClientHubSpotCommandHandlerUnitTest
{
	private readonly DeleteClientHubSpotCommandHandler _uut;
	private readonly Mock<IUnitOfWork> _fakeUnitOfWork;
	private readonly CancellationToken _cancellation;
	public DeleteClientHubSpotCommandHandlerUnitTest()
	{
		_fakeUnitOfWork = new();
		_uut = new(_fakeUnitOfWork.Object);
		_cancellation = new();
	}
	[Fact]
	public void UnitOfWorkNull_ThrowsArgumentNullexception()
	{
		// Arrange

		//Act
		Exception exception = Record.Exception(() =>
		new DeleteClientHubSpotCommandHandler(null!));

		//Assert
		Assert.Equal(typeof(ArgumentNullException), exception.GetType());

		Assert.Equal("Value cannot be null. (Parameter 'unitOfWork')", exception.Message);

	}

	[Fact]
	public async void Handle_ClientNotFound_ReturnsSuccess()
	{
		//Arrange
		DeleteClientHubSpotCommand deleteClientCommand = new(1);
		_fakeUnitOfWork.Setup(x => x
			.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(Enumerable.Empty<Client>().AsQueryable());

		//Act
		var result = await _uut.Handle(deleteClientCommand, _cancellation);

		//Assert
		_fakeUnitOfWork
			.Verify(
				unitOfWork => unitOfWork.ClientRepository
					.FindByCondition(d => d.SourceId == deleteClientCommand.ObjectId.ToString() && d.Source == Sources.HubSpot),
				Times.Once());

		_fakeUnitOfWork
			.Verify(
				unitOfWork => unitOfWork.ClientRepository.Delete(It.IsAny<Client>()),
				Times.Never());

		_fakeUnitOfWork.Verify(unitofwork => unitofwork.SaveAsync(_cancellation), Times.Never());

		Assert.True(result.IsSuccess);
		Assert.Equal(Error.None, result.Error);
	}

	[Fact]
	public async void Handle_ClientFound_ReturnsSuccess()
	{
		//Arrange
		DeleteClientHubSpotCommand deleteClientCommand = new(1);
		Client client = new();
		_fakeUnitOfWork.Setup(x => x
			.ClientRepository
			.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client>
			{
				client
			}.AsQueryable());

		//Act
		var result = await _uut.Handle(deleteClientCommand, _cancellation);

		//Assert
		_fakeUnitOfWork
		.Verify(
		unitOfWork => unitOfWork.ClientRepository
					.FindByCondition(d => d.SourceId == deleteClientCommand.ObjectId.ToString() && d.Source == Sources.HubSpot),
				Times.Once());

		_fakeUnitOfWork
			.Verify(
				unitOfWork => unitOfWork.ClientRepository.Delete(client),
				Times.Once());

		_fakeUnitOfWork.Verify(unitofwork => unitofwork.SaveAsync(_cancellation), Times.Once());

		Assert.True(result.IsSuccess);
		Assert.Equal(Error.None, result.Error);
	}
}
