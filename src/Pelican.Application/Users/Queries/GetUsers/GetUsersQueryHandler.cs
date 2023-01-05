using AutoMapper;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication;
using Pelican.Domain.Entities;

namespace Pelican.Application.Users.Queries.GetUsers;
public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, IQueryable<UserDto>>
{
	private readonly IGenericRepository<User> _repository;
	private readonly IMapper _mapper;

	public GetUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_repository = unitOfWork.UserRepository;
		_mapper = mapper;
	}
	public async Task<IQueryable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
	{
		var result = await Task.Run(() =>
			_repository.FindAll().Select(x => _mapper.Map<UserDto>(x)));

		return result.AsQueryable();
	}
}
