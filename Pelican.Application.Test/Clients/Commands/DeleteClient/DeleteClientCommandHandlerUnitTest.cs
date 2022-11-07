using Moq;
using Pelican.Application.Clients.Commands.DeleteClient;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.Clients.Commands.DeleteClient;
public class DeleteClientCommandHandlerUnitTest
{
	private readonly DeleteClientCommandHandler _uut;
	private readonly Mock<IUnitOfWork> _fakeUnitOfWork;
	private readonly CancellationToken _cancellation;
	public DeleteClientCommandHandlerUnitTest()
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
		new DeleteClientCommandHandler(null!));
		//Assert
		Assert.Equal(typeof(ArgumentNullException), exception.GetType());

		Assert.Equal("Value cannot be null. (Parameter 'IUnitOfWork')", exception.Message);

	}

	[Fact]
	public async void Handle_ClientNotFound_ReturnsSuccess()
	{
		//Arrange
		DeleteClientCommand deleteClientCommand = new(1);
		_fakeUnitOfWork.Setup(x => x
			.ClientRepository
				.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<Func<Client, bool>>>()))
			.Returns(Enumerable.Empty<Client>().AsQueryable());
		//Act
		var result = await _uut.Handle(deleteClientCommand, _cancellation);
		//Assert
		_fakeUnitOfWork.Verify(x => x.SaveAsync(_cancellation), Times.Never());

		_fakeUnitOfWork
			.Verify(
				unitOfWork => unitOfWork.ClientRepository
					.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<Func<Client, bool>>>()),
			Times.Once());

		_fakeUnitOfWork
			.Verify(
				unitOfWork => unitOfWork.ClientRepository.Delete(It.IsAny<Client>()),
				Times.Never());

		Assert.True(result.IsSuccess);
		Assert.Equal(Error.None, result.Error);
	}

	[Fact]
	public async void Handle_ClientFound_ReturnsSuccess()
	{
		//Arrange
		DeleteClientCommand deleteClientCommand = new(1);
		Client client = new();
		_fakeUnitOfWork.Setup(x => x
			.ClientRepository
			.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<Func<Client, bool>>>()))
			.Returns(new List<Client>
			{
				client
			}.AsQueryable());

		//Act
		var result = await _uut.Handle(deleteClientCommand, _cancellation);

		//Assert
		_fakeUnitOfWork.Verify(x => x.SaveAsync(_cancellation), Times.Once());

		_fakeUnitOfWork
			.Verify(
				unitOfWork => unitOfWork.ClientRepository
					.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<Func<Client, bool>>>()),
			Times.Once());

		_fakeUnitOfWork
			.Verify(
				unitOfWork => unitOfWork.ClientRepository.Delete(It.IsAny<Client>()),
				Times.Once());

		Assert.True(result.IsSuccess);
		Assert.Equal(Error.None, result.Error);
	}
}
