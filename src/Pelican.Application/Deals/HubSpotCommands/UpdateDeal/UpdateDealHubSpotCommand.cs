﻿using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Deals.HubSpotCommands.UpdateDeal;
public sealed record UpdateDealHubSpotCommand(
	long ObjectId,
	long SupplierHubSpotId,
	string PropertyName,
	string PropertyValue) : ICommand;
