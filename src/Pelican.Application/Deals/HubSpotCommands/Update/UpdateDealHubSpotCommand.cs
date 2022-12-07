using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Deals.HubSpotCommands.Update;
public sealed record UpdateDealHubSpotCommand(
	long ObjectId,
	long SupplierHubSpotId,
	long UpdateTime,
	string PropertyName,
	string PropertyValue) : ICommand;
