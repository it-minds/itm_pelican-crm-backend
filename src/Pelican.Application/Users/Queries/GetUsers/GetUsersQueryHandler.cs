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
		return await Task.FromResult(_repository
			.FindAll()
			.Select(x => _mapper.Map<UserDto>(x))
			.AsQueryable());
	}
}
