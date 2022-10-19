﻿using Microsoft.Extensions.Options;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Abstractions;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;
using Pelican.Infrastructure.HubSpot.Extensions;
using Pelican.Infrastructure.HubSpot.Mapping;
using Pelican.Infrastructure.HubSpot.Settings;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Services;

internal sealed class HubSpotContactService : HubSpotService, IHubSpotObjectService<Contact>
{
	public HubSpotContactService(
		IOptions<HubSpotSettings> hubSpotSettings) : base(hubSpotSettings)
	{ }

	public async Task<Result<Contact>> GetByIdAsync(
		string accessToken,
		long id,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"crm/v4/objects/contacts/{id}")
			.AddHeader("Authorization", $"Bearer {accessToken}")
			.AddContactQueryParams();

		RestResponse<ContactResponse> response = await _client.ExecuteGetAsync<ContactResponse>(request, cancellationToken);

		if (response.IsSuccessful
			&& response.Data is not null)
		{
			Contact result = response.Data.ToContact();

			return Result.Success(result);
		}

		return Result.Failure<Contact>(
			new Error(
				response.StatusCode.ToString(),
				response.ErrorException?.Message!));
	}

	public async Task<Result<List<Contact>>> GetAsync(
		string accessToken,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest("crm/v4/objects/contacts")
			.AddHeader("Authorization", $"Bearer {accessToken}")
			.AddContactQueryParams();

		RestResponse<ContactsResponse> response = await _client.ExecuteGetAsync<ContactsResponse>(request, cancellationToken);

		if (response.IsSuccessful
			&& response.Data is not null)
		{
			List<Contact> results = new();

			response
				.Data
				.Results
				.ToList()
				.ForEach(contactResponse =>
				{
					var result = contactResponse.ToContact();
					results.Add(result);
				});

			return Result.Success(results);
		}

		return Result.Failure<List<Contact>>(
				new Error(
					response.StatusCode.ToString(),
					response.ErrorException?.Message!));
	}
}