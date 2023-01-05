using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Users.Queries.GetUsers;
using Pelican.Domain.Entities;
using Pelican.Domain.Entities.Users;
using Xunit;

namespace Pelican.Application.Test.Users.Queries;
public class GetUsersQueryHandlerUnitTest
{
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly Mock<IGenericRepository<User>> _userRepositoryMock = new();
	private IQueryHandler<GetUsersQuery, IQueryable<User>> _uut;
	private readonly GetUsersQuery _usersQuery = new();

	[Fact]
	public async void Handle_()
	{
		//Arrange
		_unitOfWorkMock
			.Setup(x => x
				.UserRepository)
			.Returns(_userRepositoryMock.Object);

		_unitOfWorkMock
			.Setup(x => x
				.UserRepository
				.FindAll())
			.Returns(new List<User>()
			{
				new AdminUser()
				{
					Name="testName",
				}
			}.AsQueryable());

		_uut = new GetUsersQueryHandler(_unitOfWorkMock.Object);

		//Act
		var result = await _uut.Handle(_usersQuery, default);

		//Assert
		_unitOfWorkMock
			.Verify(x => x
				.UserRepository
				.FindAll(),
			Times.Once());

		Assert.Equal("testName", result.First().Name);
	}
}
