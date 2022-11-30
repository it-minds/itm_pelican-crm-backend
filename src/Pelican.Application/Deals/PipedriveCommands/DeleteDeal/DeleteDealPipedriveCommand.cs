using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Deals.PipedriveCommands.DeleteDeal;

public sealed record DeleteDealPipedriveCommand(
	int SupplierPipedriveId,
	string SubscriptionAction,
	string SubscriptionObject,
	int DealId,
	int DealPipedriveOwnerId) : ICommand;
