using AutoMapper;
using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication;
using Pelican.Application.Users.Queries.GetAllUsers;
using Pelican.Domain.Entities;
using Pelican.Domain.Entities.Users;
using Pelican.Domain.Enums;
using Xunit;

namespace Pelican.Application.Test.Users.Queries;
public class GetUsersQueryHandlerUnitTest
{
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly Mock<IGenericRepository<User>> _userRepositoryMock = new();
	private readonly Mock<IMapper> _mapperMock = new();

	private IQueryHandler<GetAllUsersQuery, IQueryable<UserDto>> _uut;
	private readonly GetAllUsersQuery _usersQuery = new();

	public GetUsersQueryHandlerUnitTest()
	{
		_unitOfWorkMock
			.Setup(x => x
				.UserRepository)
			.Returns(_userRepositoryMock.Object);

		_uut = new GetAllUsersQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
	}

	[Fact]
	public async void Handle_()
	{
		//Arrange
		Guid id = new();

		_unitOfWorkMock
			.Setup(x => x
				.UserRepository
				.FindAll())
			.Returns(new List<User>()
			{
				new AdminUser()
				{
					Name="testName",
					CreatedAt =123,
					Email ="testEmail",
					Id= id,
					LastUpdatedAt=123,
					Password="testPassword",
					SSOTokenId= "testSSOTokenId",
				}
			}.AsQueryable());

		UserDto expectedUserDto = new()
		{
			Id = id,
			Email = "testEmail",
			Name = "testName",
			Role = RoleEnum.Standard,
		};

		_mapperMock
			.Setup(x => x
				.Map<UserDto>(It.IsAny<User>()))
			.Returns(expectedUserDto);

		//Act
		var result = await _uut.Handle(_usersQuery, default);

		//Assert
		_unitOfWorkMock
			.Verify(x => x
				.UserRepository
				.FindAll(),
			Times.Once());

		Assert.Equal(expectedUserDto, result.First());
	}
}
