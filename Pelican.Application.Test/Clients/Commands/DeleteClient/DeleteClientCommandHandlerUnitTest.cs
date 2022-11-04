using Moq;
using Pelican.Application.Clients.Commands.DeleteClient;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.Clients.Commands.DeleteClient;
public class DeleteClientCommandHandlerUnitTest
{
	private DeleteClientCommandHandler uut;
	private Mock<IUnitOfWork> fakeUnitOfWork;
	private CancellationToken cancellation;
	public DeleteClientCommandHandlerUnitTest()
	{
		fakeUnitOfWork = new();
		uut = new(fakeUnitOfWork.Object);
		cancellation = new();
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
		fakeUnitOfWork.Setup(x => x
			.ClientRepository
			.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<Func<Client, bool>>>()))
			.Returns(Enumerable.Empty<Client>().AsQueryable());
		//Act
		var result = await uut.Handle(deleteClientCommand, cancellation);
		//Assert
		fakeUnitOfWork.Verify(x => x.SaveAsync(cancellation), Times.Never());

		fakeUnitOfWork
			.Verify(
				unitOfWork => unitOfWork.ClientRepository
					.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<Func<Client, bool>>>()),
			Times.Once());

		fakeUnitOfWork
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
		fakeUnitOfWork.Setup(x => x
			.ClientRepository
			.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<Func<Client, bool>>>()))
			.Returns(new List<Client>
			{
				client
			}.AsQueryable());

		//Act
		var result = await uut.Handle(deleteClientCommand, cancellation);

		//Assert
		fakeUnitOfWork.Verify(x => x.SaveAsync(cancellation), Times.Once());

		fakeUnitOfWork
			.Verify(
				unitOfWork => unitOfWork.ClientRepository
					.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<Func<Client, bool>>>()),
			Times.Once());

		fakeUnitOfWork
			.Verify(
				unitOfWork => unitOfWork.ClientRepository.Delete(It.IsAny<Client>()),
				Times.Once());

		Assert.True(result.IsSuccess);
		Assert.Equal(Error.None, result.Error);
	}
}
