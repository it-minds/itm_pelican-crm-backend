using Microsoft.AspNetCore.Http;

namespace Pelican.Presentation.Api.Utilities.HubSpotHookValidation.Extensions;

internal static class HttpRequestExtensions
{
	public static string GetBody(this HttpRequest request)
	{
		string result;

		request.EnableBuffering();

		request.Body.Position = 0;

		using (MemoryStream ms = new MemoryStream())
		{
			request.Body.CopyToAsync(ms).GetAwaiter().GetResult();
			ms.Position = 0;

			using StreamReader reader = new StreamReader(ms);
			result = reader.ReadToEndAsync().GetAwaiter().GetResult();
		}

		request.Body.Position = 0;

		return result;
	}
}
