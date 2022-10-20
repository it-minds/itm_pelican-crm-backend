using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;

namespace Pelican.Infrastructure.HubSpot.Mapping.Contacts;

internal static class ContactsResponseToContacts
{
	internal static List<Contact> ToContacts(this ContactsResponse responses)
	{
		List<Contact> results = new();

		foreach (ContactResponse response in responses.Results)
		{
			Contact result = response.ToContact();
			results.Add(result);
		}

		return results;
	}
}
