namespace Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashGenerator;

internal interface IHashGeneratorFactory
{
	IHashGenerator GetHashGenerator(int version);
}
