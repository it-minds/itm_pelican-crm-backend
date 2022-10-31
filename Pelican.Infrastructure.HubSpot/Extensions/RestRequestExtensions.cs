using Pelican.Infrastructure.HubSpot.Settings;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Extensions;

internal static class RestRequestExtensions
{
	public static RestRequest AddDealQueryParams(this RestRequest request) => request
			.AddQueryParameter("associations", "companies", false)
			.AddQueryParameter("associations", "contacts", false)
			.AddQueryParameter("properties", "hubspot_owner_id", false)
			.AddQueryParameter("properties", "closedate", false)
			.AddQueryParameter("properties", "dealstage", false)
			.AddQueryParameter("properties", "dealname", false)
			.AddQueryParameter("properties", "amount", false);

	public static RestRequest AddContactQueryParams(this RestRequest request) => request
			.AddQueryParameter("associations", "companies", false)
			.AddQueryParameter("associations", "deals", false)
			.AddQueryParameter("properties", "hubspot_owner_id", false)
			.AddQueryParameter("properties", "firstname", false)
			.AddQueryParameter("properties", "lastname", false)
			.AddQueryParameter("properties", "jobtitle", false)
			.AddQueryParameter("properties", "phone", false)
			.AddQueryParameter("properties", "email", false);

	public static RestRequest AddCompanyQueryParams(this RestRequest request) => request
			.AddQueryParameter("associations", "contacts", false)
			.AddQueryParameter("associations", "deals", false)
			.AddQueryParameter("properties", "industry", false)
			.AddQueryParameter("properties", "domain", false)
			.AddQueryParameter("properties", "name", false)
			.AddQueryParameter("properties", "city", false);

	public static RestRequest AddCommonAuthorizationQueryParams(this RestRequest request, HubSpotSettings hubSpotSettings) => request
			.AddQueryParameter("client_secret", hubSpotSettings.App.ClientSecret, false)
			.AddQueryParameter("redirect_uri", hubSpotSettings.RedirectUrl, false)
			.AddQueryParameter("client_id", hubSpotSettings.App!.ClientId, false);

	public static RestRequest AddAuthorizationHeaders(this RestRequest request) => request
			.AddHeader("Content-Type", "application/x-www-form-urlencoded")
			.AddHeader("charset", "utf-8");
}
