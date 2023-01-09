using System.Linq.Expressions;
using AutoMapper;
using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication;
using Pelican.Application.Users.Queries.GetAdmins;
using Pelican.Domain.Entities;
using Pelican.Domain.Entities.Users;
using Pelican.Domain.Enums;
using Xunit;

namespace Pelican.Application.Test.Users.Queries;
public class GetAdminsQueryHandlerUnitTest
{
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly Mock<IGenericRepository<User>> _userRepositoryMock = new();
	private readonly Mock<IMapper> _mapperMock = new();

	private IQueryHandler<GetAdminsQuery, IQueryable<UserDto>> _uut;
	private readonly GetAdminsQuery _adminsQuery = new();

	public GetAdminsQueryHandlerUnitTest()
	{
		_unitOfWorkMock
			.Setup(x => x
				.UserRepository)
			.Returns(_userRepositoryMock.Object);

		_uut = new GetAdminsQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
	}

	[Fact]
	public async void Handle_UserRepositoryFindByConditionReturnsAdminUser_ExpectedUserIsEqualToResultFirst()
	{
		//Arrange
		Guid id = new();

		_unitOfWorkMock
			.Setup(x => x
				.UserRepository
				.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
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
			Role = RoleEnum.Admin,
		};

		_mapperMock
			.Setup(x => x
				.Map<UserDto>(It.IsAny<User>()))
			.Returns(expectedUserDto);

		//Act
		var result = await _uut.Handle(_adminsQuery, default);

		//Assert
		_unitOfWorkMock
			.Verify(x => x
				.UserRepository
				.FindByCondition(x => x.Role == RoleEnum.Admin),
			Times.Once());

		Assert.Equal(expectedUserDto, result.First());
	}
}
