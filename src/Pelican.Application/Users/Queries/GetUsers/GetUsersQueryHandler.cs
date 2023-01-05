using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.Users.Queries.GetUsers;
public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, IQueryable<User>>
{
	private readonly IGenericRepository<User> _repository;

	public GetUsersQueryHandler(IUnitOfWork unitOfWork)
	{
		_repository = unitOfWork.UserRepository;
	}
	public async Task<IQueryable<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
	{
		return await Task.Run(() => _repository.FindAll());
	}
}
