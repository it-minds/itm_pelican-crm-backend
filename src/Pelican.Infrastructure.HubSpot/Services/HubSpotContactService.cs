using System.Runtime.CompilerServices;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Domain.Entities;
using Pelican.Domain.Settings.HubSpot;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;
using Pelican.Infrastructure.HubSpot.Extensions;
using Pelican.Infrastructure.HubSpot.Mapping.Contacts;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Services;

internal sealed class HubSpotContactService : ServiceBase<HubSpotSettings>, IHubSpotObjectService<Contact>
{

	public HubSpotContactService(
		IClient<HubSpotSettings> hubSpotClient,
		IUnitOfWork unitOfWork)
		: base(hubSpotClient, unitOfWork)
	{ }

	public async Task<Result<Contact>> GetByIdAsync(
		string accessToken,
		long id,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"crm/v4/objects/contacts/{id}")
			.AddHeader("Authorization", $"Bearer {accessToken}")
			.AddContactQueryParams();

		IResponse<ContactResponse> response = await _client
			.GetAsync<ContactResponse>(request, cancellationToken);

		return await response
			.GetResultWithUnitOfWork(
				ContactResponseToContact.ToContact,
				_unitOfWork,
				cancellationToken);
	}

	public async Task<Result<List<Contact>>> GetAsync(
		string accessToken,
		CancellationToken cancellationToken)
	{
		var responses = await GetAllPages(accessToken, cancellationToken).ToListAsync(cancellationToken);

		var results = responses
			.Select(response => response
				.GetResultWithUnitOfWork(
					ContactsResponseToContacts.ToContacts,
					_unitOfWork,
					cancellationToken))
			.Select(t => t.Result)
			.ToArray();

		return Result.FirstFailureOrSuccess(results) is Result result && result.IsFailure
			? (Result<List<Contact>>)result
			: results.SelectMany(r => r.Value).ToList();
	}


	private async IAsyncEnumerable<IResponse<PaginatedResponse<ContactResponse>>> GetAllPages(
		string accessToken,
		[EnumeratorCancellation] CancellationToken cancellationToken)
	{
		string after = "0";
		while (!string.IsNullOrWhiteSpace(after))
		{
			RestRequest request = new RestRequest("crm/v4/objects/contacts")
				.AddHeader("Authorization", $"Bearer {accessToken}")
				.AddContactQueryParams()
				.AddQueryParameter("after", after, false);

			IResponse<PaginatedResponse<ContactResponse>> responses = await _client
				.GetAsync<PaginatedResponse<ContactResponse>>(
					request,
					cancellationToken);

			after = responses.After();

			yield return responses;
		}
	}
}
