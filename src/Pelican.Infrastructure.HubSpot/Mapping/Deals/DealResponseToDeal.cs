﻿using Azure;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Domain;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;

namespace Pelican.Infrastructure.HubSpot.Mapping.Deals;

internal static class DealResponseToDeal
{
	public static async Task<Deal> ToDeal(
		this DealResponse response,
		IUnitOfWork unitOfWork)
	{
		if (string.IsNullOrWhiteSpace(response.Properties.HubSpotObjectId))
		{
			throw new ArgumentNullException(nameof(response));
		}

		Deal result = new()
		{
			StartDate = response.Properties.StartDate.ToUnixTimeMillisecondsOrNull(),
			EndDate = response.Properties.EndDate.ToUnixTimeMillisecondsOrNull(),
			LastContactDate = response.Properties.LastContactDate.ToUnixTimeMillisecondsOrNull(),
			SourceId = response.Properties.HubSpotObjectId,
			SourceOwnerId = response.Properties.HubSpotOwnerId,
			Name = response.Properties.DealName,
			Description = response.Properties.Description,
			Status = response.Properties.DealStage,
			Source = Sources.HubSpot,
		};

		result.SetAccountManager(await unitOfWork
			.AccountManagerRepository
			.FirstOrDefaultAsync(am => am.SourceId == result.SourceOwnerId && am.Source == Sources.HubSpot));

		result.SetContacts(response
			.Associations
			.Contacts
			.AssociationList
			.Where(association => association.Type == "deal_to_contact")
			.Select(async association => await unitOfWork
				.ContactRepository
				.FirstOrDefaultAsync(c => c.SourceId == association.Id && c.Source == Sources.HubSpot))
			.Select(t => t.Result));

		result.SetClient(response
			.Associations
			.Companies
			.AssociationList
			.Where(association => association.Type == "deal_to_company")
			.Select(async association => await unitOfWork
				.ClientRepository
				.FirstOrDefaultAsync(c => c.SourceId == association.Id && c.Source == Sources.HubSpot))
			.Select(t => t.Result)
			.Where(c => c is not null)
			.FirstOrDefault());

		return result;
	}

	private static long? ToUnixTimeMillisecondsOrNull(this string stringDate)
		=> string.IsNullOrWhiteSpace(stringDate)
		? null
		: new DateTimeOffset(Convert.ToDateTime(stringDate)).ToUnixTimeMilliseconds();
}
