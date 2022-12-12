using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Domain;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;

namespace Pelican.Infrastructure.HubSpot.Mapping.Contacts;

internal static class ContactResponseToContact
{
	public static Task<Contact> ToContact(
		this ContactResponse response,
		IUnitOfWork unitOfWork,
		CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(response.Properties.HubSpotObjectId))
		{
			throw new ArgumentNullException(nameof(response));
		}

		Contact result = new()
		{
			FirstName = response.Properties.FirstName,
			LastName = response.Properties.LastName,
			Email = response.Properties.Email,
			PhoneNumber = response.Properties.Phone,
			SourceId = response.Properties.HubSpotObjectId,
			JobTitle = response.Properties.JobTitle,
			SourceOwnerId = response.Properties.HubSpotOwnerId,
			Source = Sources.HubSpot,
		};

		result.SetClientContacts(response
			.Associations
			.Companies
			.AssociationList
			.Where(association => association.Type == "contact_to_company")
			.Select(async association => await unitOfWork
				.ClientRepository
				.FirstOrDefaultAsync(
					c => c.SourceId == association.Id && c.Source == Sources.HubSpot,
					cancellationToken))
			.Select(t => t.Result));

		result.SetDealContacts(response
			.Associations
			.Deals
			.AssociationList
			.Where(association => association.Type == "contact_to_deal")
			.Select(async association => await unitOfWork
				.DealRepository
				.FirstOrDefaultAsync(
					d => d.SourceId == association.Id && d.Source == Sources.HubSpot,
					cancellationToken))
			.Select(t => t.Result));

		return result;
	}
}
