using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;

namespace Pelican.Application.Contacts.Queries.GetContacts;
public class GetContactsQueryHandler : IQueryHandler<GetContactsQuery, IQueryable<Contact>>
{
	private readonly IGenericRepository<Contact> _repository;
	public GetContactsQueryHandler(IUnitOfWork unitOfWork)
	{
		_repository = unitOfWork.ContactRepository;
	}
	//Uses the repository for Contact to find all Contacts in the database
	public async Task<IQueryable<Contact>> Handle(GetContactsQuery request, CancellationToken cancellation)
	{
		return await Task.Run(() => _repository.FindAll());
	}
}
