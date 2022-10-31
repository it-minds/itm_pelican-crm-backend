namespace Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashComputer;

internal interface IHashComputerFactory
{
	IHashComputer CreateSHA256HashComputer();
	IHashComputer CreateClientSecretHashComputer(string clientSecret);
}
