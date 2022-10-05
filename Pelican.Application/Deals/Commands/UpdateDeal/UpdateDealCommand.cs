using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Deals.Commands.UpdateDeal;
public sealed record UpdateDealCommand(
	long UserId,
	long ObjectId,
	string? PropertyName,
	string? PropertyValue) : ICommand;
