namespace Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashComputer;

internal interface IHashComputer
{
	string ComputeHash(string text);
}
