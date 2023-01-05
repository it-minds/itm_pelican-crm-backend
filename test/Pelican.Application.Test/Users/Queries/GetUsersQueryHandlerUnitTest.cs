using AutoMapper;
using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication;
using Pelican.Application.Users.Queries.GetUsers;
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
	private IQueryHandler<GetUsersQuery, IQueryable<UserDto>> _uut;
	private readonly GetUsersQuery _usersQuery = new();

	[Fact]
	public async void Handle_()
	{
		//Arrange
		_unitOfWorkMock
			.Setup(x => x
				.UserRepository)
			.Returns(_userRepositoryMock.Object);

		Guid id = new Guid();

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

		_uut = new GetUsersQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);

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
