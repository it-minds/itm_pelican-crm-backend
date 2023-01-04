using System.Linq.Expressions;
using Moq;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Authentication;
using Pelican.Application.Authentication.ResetPassword;
using Pelican.Domain.Entities;
using Pelican.Domain.Entities.Users;
using Pelican.Domain.Enums;
using Xunit;

namespace Pelican.Application.Test.ResetPassword.Command;
public class ResetPasswordCommandHandlerTests
{
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly Mock<ITokenService> _tokenServiceMock = new();
	private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
	private readonly ResetPasswordCommandHandler _uut;

	public ResetPasswordCommandHandlerTests()
	{
		_uut = new(_unitOfWorkMock.Object, _tokenServiceMock.Object, _passwordHasherMock.Object);
	}

	[Fact]
	public void test1()
	{
		//Act
		ResetPasswordCommandHandler _uut;
		var result = Record.Exception(() => _uut = new(null!, _tokenServiceMock.Object, _passwordHasherMock.Object));

		//Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains("unitOfWork", result.Message);
	}

	[Fact]
	public void test2()
	{
		//Act
		ResetPasswordCommandHandler _uut;
		var result = Record.Exception(() => _uut = new(_unitOfWorkMock.Object, null!, _passwordHasherMock.Object));

		//Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains("tokenService", result.Message);
	}

	[Fact]
	public void test3()
	{
		//Act
		ResetPasswordCommandHandler _uut;
		var result = Record.Exception(() => _uut = new(_unitOfWorkMock.Object, _tokenServiceMock.Object, null!));

		//Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains("passwordHasher", result.Message);
	}

	[Fact]
	public void test4()
	{
		//Arrange
		ResetPasswordCommand testResetPasswordCommand = new("testSSOToken", "testNewPassword");

		_tokenServiceMock
			.Setup(x => x
				.ValidateSSOToken(It.IsAny<string>()))
			.Throws(new ArgumentOutOfRangeException());

		//Act
		var result = Record.ExceptionAsync(async () => await _uut.Handle(testResetPasswordCommand, default));

		//Assert
		_tokenServiceMock
			.Verify(x => x
				.ValidateSSOToken("testSSOToken"), Times.Once);

		Assert.IsType<ArgumentException>(result.Result);

		Assert.Contains(new ArgumentOutOfRangeException().Message, result.Result.Message);

		Assert.Contains(": The provided token was invalid.", result.Result.Message);
	}

	[Fact]
	public void test5()
	{
		//Arrange
		ResetPasswordCommand testResetPasswordCommand = new("testSSOToken", "testNewPassword");

		_tokenServiceMock
			.Setup(x => x
				.ValidateSSOToken(It.IsAny<string>()))
			.Returns(("returnedUserEmail", "returnedTokenId"));

		_unitOfWorkMock
			.Setup(x => x
				.UserRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<User, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync((User)null!);

		//Act
		var result = Record.ExceptionAsync(async () => await _uut.Handle(testResetPasswordCommand, default));

		//Assert
		_unitOfWorkMock
			.Verify(x => x
				.UserRepository
				.FirstOrDefaultAsync(x => x.Email == "returnedUserEmail", default), Times.Once);

		Assert.IsType<ArgumentNullException>(result.Result);

		Assert.Contains("userEntity", result.Result.Message);
	}

	[Fact]
	public void test6()
	{
		//Arrange
		ResetPasswordCommand testResetPasswordCommand = new("testSSOToken", "testNewPassword");

		_tokenServiceMock
			.Setup(x => x
				.ValidateSSOToken(It.IsAny<string>()))
			.Returns(("returnedUserEmail", "returnedTokenId"));

		_unitOfWorkMock
			.Setup(x => x
				.UserRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<User, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync(new AdminUser()
			{
				SSOTokenId = "expectedTokenId"
			});

		//Act
		var result = Record.ExceptionAsync(async () => await _uut.Handle(testResetPasswordCommand, default));

		//Assert
		_unitOfWorkMock
			.Verify(x => x
				.UserRepository
				.FirstOrDefaultAsync(x => x.Email == "returnedUserEmail", default), Times.Once);

		Assert.IsType<ArgumentException>(result.Result);

		Assert.Contains($"The provided token did not match the expected token. Expected 'expectedTokenId' found 'returnedTokenId'.", result.Result.Message);
	}

	[Fact]
	public async Task test7Async()
	{
		//Arrange
		ResetPasswordCommand testResetPasswordCommand = new("testSSOToken", "testNewPassword");

		Guid userId = new Guid();

		_tokenServiceMock
			.Setup(x => x
				.ValidateSSOToken(It.IsAny<string>()))
			.Returns(("returnedUserEmail", "returnedTokenId"));

		_unitOfWorkMock
			.Setup(x => x
				.UserRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<User, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync(new AdminUser()
			{
				Email = "returnedEmail",
				Name = "returnedName",
				Id = userId,
				CreatedAt = 123,
				LastUpdatedAt = 142,
				Password = "oldPassword",
				Role = RoleEnum.Admin,
				SSOTokenId = "returnedTokenId"
			});

		_passwordHasherMock
			.Setup(x => x
				.Hash(It.IsAny<string>()))
			.Returns("hashedNewPassword");

		_tokenServiceMock
			.Setup(x => x
				.CreateToken(It.IsAny<User>()))
			.Returns("returnedToken");

		AdminUser expectedUser = new()
		{
			Id = userId,
			Password = "hashedNewPassword",
			Email = "returnedEmail",
			Name = "returnedName",
			CreatedAt = 123,
			LastUpdatedAt = 142,
			Role = RoleEnum.Admin,
		};

		UserTokenDto expectedUserDTO = new()
		{
			User = new()
			{
				Email = expectedUser.Email,
				Id = expectedUser.Id,
				Name = expectedUser.Name,
				Role = expectedUser.Role,
			},
			Token = "returnedToken",
		};

		//Act
		var result = await _uut.Handle(testResetPasswordCommand, default);

		//Assert
		_unitOfWorkMock
			.Verify(x => x
				.UserRepository
				.FirstOrDefaultAsync(x => x.Email == "returnedUserEmail", default), Times.Once);

		_unitOfWorkMock
			.Verify(x => x
				.UserRepository
				.Update(expectedUser), Times.Once);

		_unitOfWorkMock
			.Verify(x => x
				.SaveAsync(default), Times.Once);

		Assert.Equivalent(expectedUserDTO, result.Value);
	}
}
