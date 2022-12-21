using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Domain;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Clients;

namespace Pelican.Infrastructure.HubSpot.Mapping.Clients;

internal static class CompanyResponseToClient
{
	public static async Task<Client> ToClient(
		this CompanyResponse response,
		IUnitOfWork unitOfWork,
		CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(response.Properties.HubSpotObjectId)
			|| string.IsNullOrWhiteSpace(response.Properties.Name))
		{
			throw new ArgumentNullException(nameof(response));
		}

		Client result = new()
		{
			SourceId = response.Properties.HubSpotObjectId,
			Name = response.Properties.Name,
			Website = response.Properties.Domain,
			OfficeLocation = response.Properties.City,
			Source = Sources.HubSpot,
		};

		result.SetDeals(response.Associations
			.Deals
			.AssociationList
			.Where(association => association.Type == "company_to_deal_unlabeled")
			.Select(async association => await unitOfWork
				.DealRepository
				.FirstOrDefaultAsync(
					d => d.SourceId == association.Id && d.Source == Sources.HubSpot,
					cancellationToken))
			.Select(t => t.Result)
			.ToList());

		result.SetClientContacts(response
			.Associations
			.Contacts
			.AssociationList
			.Where(association => association.Type == "company_to_contact_unlabeled")
			.Select(async association => await unitOfWork
				.ContactRepository
				.FirstOrDefaultAsync(
					c => c.SourceId == association.Id && c.Source == Sources.HubSpot,
					cancellationToken))
			.Select(t => t.Result)
			.ToList());

		return await Task.FromResult(result);
	}
}
