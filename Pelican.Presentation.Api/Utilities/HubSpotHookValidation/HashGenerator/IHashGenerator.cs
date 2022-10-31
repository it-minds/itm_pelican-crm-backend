namespace Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashGenerator;

internal interface IHashGenerator
{
	string GenerateHash(HttpRequest request);
}
