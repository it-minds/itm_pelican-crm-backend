using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Deals.Commands.UpdateDeal;
public sealed record UpdateDealCommand(
	long ObjectId,
	long PortalId,
	string PropertyName,
	string PropertyValue) : ICommand;
