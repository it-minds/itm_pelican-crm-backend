using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Deals.HubSpotCommands.Delete;

public sealed record DeleteDealHubSpotCommand(
	long ObjectId) : ICommand;
