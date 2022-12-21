using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;

namespace Pelican.Infrastructure.HubSpot.Mapping.Contacts;

internal static class ContactsResponseToContacts
{
	internal static async Task<List<Contact>> ToContacts(
		this PaginatedResponse<ContactResponse> responses,
		IUnitOfWork unitOfWork,
		CancellationToken cancellationToken)
	{
		if (responses.Results is null)
		{
			throw new ArgumentNullException(nameof(responses));
		}

		List<Contact> results = new();

		foreach (ContactResponse response in responses.Results)
		{
			Contact result = await response.ToContact(unitOfWork, cancellationToken);
			results.Add(result);
		}

		return results;
	}
}
