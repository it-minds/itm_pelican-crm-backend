using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Deals.PipedriveCommands.Delete;

public sealed record DeleteDealPipedriveCommand(int DealId) : ICommand;
