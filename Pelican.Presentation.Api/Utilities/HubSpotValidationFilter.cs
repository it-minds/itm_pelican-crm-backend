using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Pelican.Infrastructure.HubSpot;

namespace Pelican.Presentation.Api.Utilities;

// https://developers.hubspot.com/docs/api/webhooks/validating-requests
public sealed class HubSpotValidationFilter : IResourceFilter
{
	private readonly ILogger<HubSpotValidationFilter> _logger;
	private readonly HubSpotSettings _settings;
	public HubSpotValidationFilter(IOptions<HubSpotSettings> options, ILogger<HubSpotValidationFilter> logger)
	{
		_logger = logger;
		_settings = options.Value;
	}

	public void OnResourceExecuting(ResourceExecutingContext context)
	{
		var hasSignature =
			context.HttpContext.Request.Headers.TryGetValue("X-HubSpot-Signature", out var signature);
		var hasSignatureVersion = context.HttpContext.Request.Headers.TryGetValue("X-HubSpot-Signature-Version", out var signatureVersion);

		if (!hasSignature || !hasSignatureVersion)
		{
			context.Result = new BadRequestObjectResult(new { message = "Unable to validate signature" });
			_logger.LogWarning("HubSpot Signature validation failed. Has signature: \"{hasSignature}\" Has signature version: \"{hasSignatureVersion}\"", hasSignature, signatureVersion);
			return;
		}

		var version = int.Parse(signatureVersion.ToString().Trim('v'));
		var hash = version switch
		{
			1 => GenerateV1Hash(context.HttpContext.Request),
			2 => GenerateV2Hash(context.HttpContext.Request),
			3 => GenerateV3Hash(context.HttpContext.Request),
			_ => throw new Exception("Unsupported signature version")
		};

		if (hash != signature.ToString())
		{
			context.Result = new BadRequestObjectResult(new { message = "Invalid request signature" });
			_logger.LogWarning("HubSpot Signature validation failed. Invalid signature");
		}
	}

	public void OnResourceExecuted(ResourceExecutedContext context)
	{

	}

	private string GenerateV1Hash(HttpRequest request)
	{
		var builder = new StringBuilder(_settings?.App?.ClientSecret);

		if (request.ContentLength > 0)
		{
			var body = GetBodyFromRequest(request);
			builder.Append(body);
		}

		return ComputeHash(builder.ToString());
	}

	private string GenerateV2Hash(HttpRequest request)
	{
		var builder = new StringBuilder(_settings?.App?.ClientSecret);
		builder.Append(request.Method);
		builder.Append(request.GetEncodedUrl());

		if (request.ContentLength > 0)
		{
			var body = GetBodyFromRequest(request);
			builder.Append(body);
		}

		return ComputeHash(builder.ToString());
	}

	private string GenerateV3Hash(HttpRequest request)
	{
		var hasTimestamp = request.Headers.TryGetValue("X-HubSpot-Request-Timestamp", out var timestamp);
		if (!hasTimestamp || Convert.ToInt64(timestamp) < DateTimeOffset.Now.AddMinutes(-5).ToUnixTimeMilliseconds())
		{
			return "";
		}

		var builder = new StringBuilder(request.Method);
		builder.Append(request.GetEncodedUrl());

		if (request.ContentLength > 0)
		{
			var body = GetBodyFromRequest(request);
			builder.Append(body);
		}

		builder.Append(timestamp);

		return ComputeHashWithClientSecretAsSecret(builder.ToString());
	}

	private string ComputeHash(string text)
	{
		using var sha = SHA256.Create();
		byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(text));

		var hashBuilder = new StringBuilder();

		foreach (var b in bytes)
		{
			hashBuilder.Append(b.ToString("x2"));
		}

		return hashBuilder.ToString();
	}

	private string ComputeHashWithClientSecretAsSecret(string text)
	{
		using var hmac = new HMACSHA256(Encoding.ASCII.GetBytes(_settings.App?.ClientSecret ?? ""));
		byte[] bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(text));

		return Convert.ToBase64String(bytes);
	}

	private static string GetBodyFromRequest(HttpRequest request)
	{
		string result;
		request.EnableBuffering();
		request.Body.Position = 0;
		using (var ms = new MemoryStream())
		{
			request.Body.CopyToAsync(ms).GetAwaiter().GetResult();
			ms.Position = 0;

			using var reader = new StreamReader(ms);
			result = reader.ReadToEndAsync().GetAwaiter().GetResult();
		}
		request.Body.Position = 0;

		return result;
	}
}
