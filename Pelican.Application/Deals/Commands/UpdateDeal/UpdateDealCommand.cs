using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Deals.Commands.UpdateDeal;
public sealed record UpdateDealCommand(
	long ObjectId,
	string? UserId,
	string? PropertyName,
	string? PropertyValue) : ICommand;
