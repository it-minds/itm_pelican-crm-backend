using AutoMapper;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication;
using Pelican.Domain.Entities;
using Pelican.Domain.Enums;

namespace Pelican.Application.Users.Queries.GetAdmins;
public class GetAdminsQueryHandler : IQueryHandler<GetAdminsQuery, IQueryable<UserDto>>
{
	private readonly IGenericRepository<User> _repository;
	private readonly IMapper _mapper;

	public GetAdminsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_repository = unitOfWork.UserRepository;
		_mapper = mapper;
	}
	public async Task<IQueryable<UserDto>> Handle(GetAdminsQuery request, CancellationToken cancellationToken)
	{
		var result = await Task.Run(() =>
			_repository.FindByCondition(x => x.Role == RoleEnum.Admin).Select(x => _mapper.Map<UserDto>(x)));

		return result.AsQueryable();
	}
}
