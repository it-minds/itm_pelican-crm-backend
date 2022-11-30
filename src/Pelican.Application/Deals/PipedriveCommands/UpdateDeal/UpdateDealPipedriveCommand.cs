using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Deals.PipedriveCommands.UpdateDeal;
public sealed record UpdateDealPipedriveCommand(
	int SupplierPipedriveId,
	int DealId,
	int DealPipedriveOwnerId,
	int DealStatusId,
	string? DealDescription,
	string? DealName,
	string? LastContactDate,
	string? StartDate,
	string? EndDate) : ICommand;
