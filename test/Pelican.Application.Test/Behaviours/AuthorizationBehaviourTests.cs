using MediatR;
using Moq;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication;
using Pelican.Application.Behaviours;
using Pelican.Application.Security;
using Pelican.Domain.Entities;
using Pelican.Domain.Enums;
using Xunit;

namespace Pelican.Application.Test.Behaviours;
public class AuthorizationBehaviourTests
{
	[Authorize(Role = Domain.Enums.RoleEnum.Standard)]
	public record testQuery : IQuery<IQueryable<Contact>>
	{
	}

	private Mock<ICurrentUserService> _userServiceMock = new();
	private Mock<IAuthorizationService> _authorizationServiceMock = new();
	private Mock<RequestHandlerDelegate<testQuery>> _delegateMock = new();
	private Mock<IGetCustomAttributesService> _getCustomAttributesServiceMock = new();
	private Mock<IRequest<testQuery>> _queryMock = new();

	private AuthorizationBehaviour<IRequest<testQuery>, testQuery> _uut;

	public AuthorizationBehaviourTests()
	{
		_uut = new(_userServiceMock.Object, _authorizationServiceMock.Object, _getCustomAttributesServiceMock.Object);
	}


	[Fact]
	public async void Handle_GetCustomAttributesServiceReturnsEmptyList_ResultIsNull()
	{
		//Arrange
		_getCustomAttributesServiceMock
			.Setup(x => x
				.GetAttributes(
					It.IsAny<IRequest>()))
			.Returns(new List<AuthorizeAttribute>().AsEnumerable());

		//Act
		var result = await _uut.Handle(_queryMock.Object, _delegateMock.Object, default);

		//Assert
		Assert.Null(result);
	}

	[Fact]
	public void Handle_GetCustomAttributsServiceReturnsANonEmptyListCurrentUserServiceReturnsNull_UnauthorizedExceptionsIsThrown()
	{
		//Arrange
		_getCustomAttributesServiceMock
			.Setup(x => x
				.GetAttributes(
					It.IsAny<IRequest<testQuery>>()))
			.Returns(new List<AuthorizeAttribute>()
			{
				new AuthorizeAttribute(),
			});

		_userServiceMock
			.SetupGet(x => x
				.UserId)
			.Returns((string)null!);

		//Act
		var result = Record.ExceptionAsync(async () => await _uut.Handle(_queryMock.Object, _delegateMock.Object, default));

		//Assert
		Assert.IsType<UnauthorizedAccessException>(result.Result);
	}

	[Fact]
	public void Handle_CurrentUserServiceReturnsAnUserIdAuthorizationServiceReturnsFalse_ForbiddenAccessExceptionIsThrown()
	{
		//Arrange
		_getCustomAttributesServiceMock
			.Setup(x => x
				.GetAttributes(
					It.IsAny<IRequest<testQuery>>()))
			.Returns(new List<AuthorizeAttribute>()
			{
				new AuthorizeAttribute(),
			});

		_userServiceMock
			.SetupGet(x => x
				.UserId)
			.Returns("NotNullOrWhiteSpace");

		_authorizationServiceMock
			.Setup(x => x
				.IsInRole(It.IsAny<RoleEnum>()))
			.Returns(false);

		//Act
		var result = Record.ExceptionAsync(async () => await _uut.Handle(_queryMock.Object, _delegateMock.Object, default));

		//Assert
		Assert.IsType<ForbiddenAccessException>(result.Result);
	}

	[Fact]
	public async Task Handle_AuthorizationServiceReturnsTrue_ResultIsNull()
	{
		//Arrange
		_getCustomAttributesServiceMock
			.Setup(x => x
				.GetAttributes(
					It.IsAny<IRequest<testQuery>>()))
			.Returns(new List<AuthorizeAttribute>()
			{
				new AuthorizeAttribute(),
			});

		_userServiceMock
			.SetupGet(x => x
				.UserId)
			.Returns("NotNullOrWhiteSpace");

		_authorizationServiceMock
			.Setup(x => x
				.IsInRole(It.IsAny<RoleEnum>()))
			.Returns(true);

		//Act
		var result = await _uut.Handle(_queryMock.Object, _delegateMock.Object, default);

		//Assert
		Assert.Null(result);
	}
}
