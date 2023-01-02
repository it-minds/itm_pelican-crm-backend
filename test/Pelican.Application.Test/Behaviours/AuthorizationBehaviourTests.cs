using MediatR;
using Moq;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Behaviours;
using Pelican.Application.Security;
using Pelican.Domain.Entities;
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
	private AuthorizationBehaviour<IRequest<testQuery>, testQuery> _uut;
	private Mock<RequestHandlerDelegate<testQuery>> _delegateMock = new();
	Mock<IQuery<testQuery>> _queryMock = new();


	public AuthorizationBehaviourTests()
	{
		_uut = new(_userServiceMock.Object, _authorizationServiceMock.Object);
	}


	[Fact]
	public async void Test()
	{
		//Arrange
		//_queryMock.Setup(x => x.GetType().).Returns(typeof(testQuery));

		//Act
		var result = await _uut.Handle(_queryMock.Object, _delegateMock.Object, default);

		//Assert

	}
}
