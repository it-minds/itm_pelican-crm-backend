using Moq;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication.CreateAdmin;
using Pelican.Domain.Entities;
using Pelican.Domain.Entities.Users;
using Xunit;

namespace Pelican.Application.Test.CreateAdmin.Commands;
public class CreateAdminCommandHandlerTests
{
	private readonly ICommandHandler<CreateAdminCommand> _uut;
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly Mock<IPasswordHasher> _passwordHasherMock = new();

	public CreateAdminCommandHandlerTests()
	{
		_uut = new CreateAdminCommandHandler(_unitOfWorkMock.Object, _passwordHasherMock.Object);
	}

	[Fact]
	public void Constructor_UnitOfWorkNull_ArgumentNullException()
	{
		//Arrange

		//Act
		var result = Record.Exception(() => new CreateAdminCommandHandler(null!, _passwordHasherMock.Object));

		//Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains(
			"unitOfWork",
			result.Message);
	}

	[Fact]
	public void Constructor_PassWordHasherNull_ArgumentNullException()
	{
		//Arrange

		//Act
		var result = Record.Exception(() => new CreateAdminCommandHandler(_unitOfWorkMock.Object, null!));

		//Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains(
			"passwordHasher",
			result.Message);

	}

	[Fact]
	public async void Handler()
	{
		//Arrange
		CreateAdminCommand createAdminCommand = new("test", "test", "test");

		_passwordHasherMock.Setup(x => x.Hash(It.IsAny<string>())).Returns("HashedPW");

		_unitOfWorkMock.Setup(x => x.UserRepository.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()));

		AdminUser expectedUser = new()
		{
			Name = "test",
			Password = "HashedPW",
			Email = "test",
		};

		//Act
		var result = await _uut.Handle(createAdminCommand, default);

		//Assert
		_unitOfWorkMock.Verify(x => x.UserRepository.CreateAsync(expectedUser, default), Times.Once);

		_unitOfWorkMock.Verify(x => x.SaveAsync(default), Times.Once);

		_passwordHasherMock.Verify(x => x.Hash("test"));

		Assert.True(result.IsSuccess);
	}
}
