namespace Pelican.Application.Deals.HubSpotCommands.UpdateDeal;
public sealed record UpdateDealHubSpotCommand(
>>>>>>> Added enpoint for update Pipedrive deal: src/Pelican.Application/Deals/HubSpotCommands/UpdateDeal/UpdateDealHubSpotCommand.cs
	long ObjectId,
	long SupplierHubSpotId,
	string PropertyName,
	string PropertyValue) : ICommand;
