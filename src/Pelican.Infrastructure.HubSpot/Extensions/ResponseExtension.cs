using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;

namespace Pelican.Infrastructure.HubSpot.Extensions;

internal static class ResponseExtension
{
	internal static string After<TResponse>(this IResponse<PaginatedResponse<TResponse>> response)
		where TResponse : class
		=> response.Data?.Paging.Next.After ?? string.Empty;
}
