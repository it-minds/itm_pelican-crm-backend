using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Deals.Commands.UpdateDeal;
public sealed record UpdateDealPipedriveCommand(
	int SupplierPipedriveId,
	string SubscriptionAction,
	string SubscriptionObject,
	int DealStatusId,
	string? DealDescription,
	string? DealName,
	string? LastContactDate,
	int DealId,
	int DealPipedriveOwnerId) : ICommand;
