﻿using MediatR;
using Moq;
using Pelican.Application.Users.Queries.GetAllUsers;
using Pelican.Presentation.GraphQL.Users;
using Xunit;

namespace Pelican.Presentation.GraphQL.Test;
public class UsersQueryUnitTest
{
	private readonly AllUsersQuery _uut = new();
	private readonly Mock<IMediator> _mediatorMock = new();

	[Fact]
	public void GetUsers()
	{
		//Act
		_ = _uut.GetUsersAsync(_mediatorMock.Object, default);

		//Assert
		_mediatorMock.Verify(
			x => x.Send(It.IsAny<GetAllUsersQuery>(), default),
			Times.Once);
	}
}
