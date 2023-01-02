using MediatR;
using Moq;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Behaviours;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Application.Test.Behaviours;
public class AuthorizationBehaviourTests
{
	private Mock<ICurrentUserService> _userServiceMock = new();
	private Mock<IAuthorizationService> _authorizationServiceMock = new();
	private AuthorizationBehaviour<IRequest<Deal>, Deal> _uut;
	private Mock<RequestHandlerDelegate<Deal>> _delegateMock = new();
	Mock<IQuery<Deal>> _queryMock = new();


	public AuthorizationBehaviourTests()
	{
		_uut = new(_userServiceMock.Object, _authorizationServiceMock.Object);
	}

	public class testClass
	{

		public testClass()
		{

		}
	}

	[Fact]
	public async void Test()
	{
		//Arrange


		//Act
		var result = await _uut.Handle(_queryMock.Object, _delegateMock.Object, default);

		//Assert

	}
}
