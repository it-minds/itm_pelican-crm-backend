namespace Pelican.Application.Deals.Commands.UpdateDeal;
public sealed record UpdateDealCommand(
	long ObjectId,
	long SupplierHubSpotId,
	string PropertyName,
	string PropertyValue) : ICommand;
