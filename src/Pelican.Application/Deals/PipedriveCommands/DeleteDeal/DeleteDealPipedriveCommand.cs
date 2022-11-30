using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Deals.PipedriveCommands.DeleteDeal;

public sealed record DeleteDealPipedriveCommand(
	int SupplierPipedriveId,
	int DealId,
	int DealPipedriveOwnerId) : ICommand;
