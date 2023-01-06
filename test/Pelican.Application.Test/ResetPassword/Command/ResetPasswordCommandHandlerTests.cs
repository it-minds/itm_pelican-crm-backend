using System.Linq.Expressions;
using AutoMapper;
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
	private readonly Mock<IMapper> _mapperMock = new();
	private readonly ResetPasswordCommandHandler _uut;

	public ResetPasswordCommandHandlerTests()
	{
		_uut = new(_unitOfWorkMock.Object, _tokenServiceMock.Object, _passwordHasherMock.Object, _mapperMock.Object);
	}

	[Fact]
	public void Constructor_unitOfWorkIsNull_ThrowsArgumentNullException()
	{
		//Act
		ResetPasswordCommandHandler _uut;
		var result = Record.Exception(() => _uut = new(null!, _tokenServiceMock.Object, _passwordHasherMock.Object, _mapperMock.Object));

		//Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains("unitOfWork", result.Message);
	}

	[Fact]
	public void Constructor_tokenServiceIsNull_ThrowsArgumentNullException()
	{
		//Act
		ResetPasswordCommandHandler _uut;
		var result = Record.Exception(() => _uut = new(_unitOfWorkMock.Object, null!, _passwordHasherMock.Object, _mapperMock.Object));

		//Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains("tokenService", result.Message);
	}

	[Fact]
	public void Constructor_passWordHasherIsNull_ThrowsArgumentNullException()
	{
		//Act
		ResetPasswordCommandHandler _uut;
		var result = Record.Exception(() => _uut = new(_unitOfWorkMock.Object, _tokenServiceMock.Object, null!, _mapperMock.Object));

		//Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains("passwordHasher", result.Message);
	}

	[Fact]
	public void Constructor_mapperIsNull_ThrowsArgumentNullException()
	{
		//Act
		ResetPasswordCommandHandler _uut;
		var result = Record.Exception(() => _uut = new(_unitOfWorkMock.Object, _tokenServiceMock.Object, _passwordHasherMock.Object, null!));

		//Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains("mapper", result.Message);
	}

	[Fact]
	public async Task Handle_TokenServiceValidateSSOTokenReturnsError_ResultIsFailure()
	{
		//Arrange
		ResetPasswordCommand testResetPasswordCommand = new("testSSOToken", "testNewPassword");

		_tokenServiceMock
			.Setup(x => x
				.ValidateSSOToken(It.IsAny<string>()))
			.Throws(new ArgumentOutOfRangeException());

		//Act
		var result = await _uut.Handle(testResetPasswordCommand, default);

		//Assert
		_tokenServiceMock
			.Verify(x => x
				.ValidateSSOToken("testSSOToken"), Times.Once);

		Assert.True(result.IsFailure);

		Assert.Contains("TokenService.Exception", result.Error.Code);

		Assert.Contains("The provided token was invalid.", result.Error.Message);
	}

	[Fact]
	public async Task Handle_UserRepositoryReturnsNullForUser_ResultIsFailure()
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
		var result = await _uut.Handle(testResetPasswordCommand, default);

		//Assert
		_unitOfWorkMock
			.Verify(x => x
				.UserRepository
				.FirstOrDefaultAsync(x => x.Email == "returnedUserEmail", default), Times.Once);

		Assert.True(result.IsFailure);

		Assert.Contains("User is null", result.Error.Message);

		Assert.Contains("User.Null", result.Error.Code);
	}

	[Fact]
	public async Task Handle_UserFromRepoHasDifferentTokenIdThanTokenValidated_ResultIsFailure()
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
		var result = await _uut.Handle(testResetPasswordCommand, default);

		//Assert
		_unitOfWorkMock
			.Verify(x => x
				.UserRepository
				.FirstOrDefaultAsync(x => x.Email == "returnedUserEmail", default), Times.Once);

		Assert.True(result.IsFailure);

		Assert.Contains($"The provided token did not match the expected token. Expected 'expectedTokenId' found 'returnedTokenId'.", result.Error.Message);

		Assert.Contains("User.TokenIdArgument", result.Error.Code);
	}

	[Fact]
	public async Task Handle_PasswordIsSuccefullyReset_ResultIsSuccess()
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

		User expectedUser = new AdminUser()
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

		_mapperMock
			.Setup(x => x
				.Map<UserTokenDto>(It.IsAny<User>()))
			.Returns(expectedUserDTO);

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

		Assert.True(result.IsSuccess);

		Assert.Equivalent(expectedUserDTO, result.Value);
	}
}
