using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Deals.Commands.DeleteDeal;

public sealed record DeleteDealCommand(
	long ObjectId) : ICommand;
