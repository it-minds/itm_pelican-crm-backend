﻿using MediatR;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Application.Contacts.Queries.GetContacts;
public class GetContactsQueryHandler : IRequestHandler<GetContactsQuery, IQueryable<Contact>>
{
	private readonly IContactRepository _repository;
	public GetContactsQueryHandler(IUnitOfWork unitOfWork)
	{
		_repository = unitOfWork.ContactRepository;
	}
	public async Task<IQueryable<Contact>> Handle(GetContactsQuery request, CancellationToken cancellation)
	{
		return _repository.FindAll();
	}
}
