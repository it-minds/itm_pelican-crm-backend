namespace Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashComputer;

internal sealed class HashComputerFactory : IHashComputerFactory
{
	public IHashComputer CreateSHA256HashComputer() => new SHA256HashComputer();

	public IHashComputer CreateClientSecretHashComputer(string clientSecret) => new ClientSecretHashComputer(clientSecret);
}
