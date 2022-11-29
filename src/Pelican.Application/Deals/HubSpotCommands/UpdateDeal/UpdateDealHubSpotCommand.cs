using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Deals.Commands.UpdateDeal;
public sealed record UpdateDealHubSpotCommand(
	long ObjectId,
	long SupplierHubSpotId,
	string PropertyName,
	string PropertyValue) : ICommand;
