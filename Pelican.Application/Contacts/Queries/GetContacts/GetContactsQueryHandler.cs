using MediatR;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Application.Contacts.Queries.GetContacts;
public class GetContactsQueryHandler : IRequestHandler<GetContactsQuery, IQueryable<Contact>>
{
	private readonly IRepositoryBase<Contact> _repository;
	public GetContactsQueryHandler(IUnitOfWork unitOfWork)
	{
		_repository = unitOfWork.ContactRepository;
	}
	//Uses the repository for Contact to find all Contacts in the database
	public async Task<IQueryable<Contact>> Handle(GetContactsQuery request, CancellationToken cancellation)
	{
		return _repository.FindAll();
	}
}
