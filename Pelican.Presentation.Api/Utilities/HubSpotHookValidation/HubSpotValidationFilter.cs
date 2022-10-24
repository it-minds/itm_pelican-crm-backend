using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashGenerator;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Pelican.Presentation.Api.Utilities.HubSpotHookValidation;

// https://developers.hubspot.com/docs/api/webhooks/validating-requests
internal sealed class HubSpotValidationFilter : IResourceFilter
{
	private readonly IHashGeneratorFactory _hashGeneratorFactory;
	private readonly ILogger<HubSpotValidationFilter> _logger;

	public HubSpotValidationFilter(
		IHashGeneratorFactory hashGeneratorFactory,
		ILogger<HubSpotValidationFilter> logger)
	{
		_logger = logger;
		_hashGeneratorFactory = hashGeneratorFactory;
	}

	public void OnResourceExecuting(ResourceExecutingContext context)
	{
		bool hasSignature =
			context.HttpContext.Request.Headers.TryGetValue(
				"X-HubSpot-Signature",
				out StringValues signature);

		bool hasSignatureVersion =
			context.HttpContext.Request.Headers.TryGetValue(
				"X-HubSpot-Signature-Version",
				out StringValues signatureVersion);

		if (!hasSignature
			|| !hasSignatureVersion)
		{
			context.Result = new BadRequestObjectResult(new { message = "Unable to validate signature" });

			_logger.LogWarning(
				"HubSpot Signature validation failed. Has signature: \"{hasSignature}\" Has signature version: \"{hasSignatureVersion}\"",
				hasSignature,
				signatureVersion);

			return;
		}

		int version = int.Parse(signatureVersion.ToString().Trim('v'));

		IHashGenerator hashGenerator = _hashGeneratorFactory.GetHashGenerator(version);

		string hash = hashGenerator.GenerateHash(context.HttpContext.Request);

		if (hash != signature.ToString())
		{
			context.Result = new BadRequestObjectResult(new { message = "Invalid request signature" });

			_logger.LogWarning("HubSpot Signature validation failed. Invalid signature");
		}
	}

	public void OnResourceExecuted(ResourceExecutedContext context) { }

}
