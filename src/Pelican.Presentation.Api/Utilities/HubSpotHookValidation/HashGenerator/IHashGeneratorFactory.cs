namespace Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashGenerator;

internal interface IHashGeneratorFactory
{
	IHashGenerator CreateHashGenerator(int version);
}
