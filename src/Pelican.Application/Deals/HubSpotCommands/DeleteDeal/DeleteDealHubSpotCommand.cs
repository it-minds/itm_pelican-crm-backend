
using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Deals.HubSpotCommands.DeleteDeal;

public sealed record DeleteDealHubSpotCommand(
	long ObjectId) : ICommand;
