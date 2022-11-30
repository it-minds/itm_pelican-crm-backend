﻿using FluentValidation;
using Pelican.Application.Deals.HubSpotCommands.UpdateDeal;

namespace Pelican.Application.Contacts.Commands.UpdateContact;

internal sealed class UpdateDealHubSpotCommandValidator : AbstractValidator<UpdateDealHubSpotCommand>
{
	public UpdateDealHubSpotCommandValidator()
	{
		RuleFor(command => command.ObjectId).NotEmpty();
		RuleFor(command => command.SupplierHubSpotId).NotEmpty();
		RuleFor(command => command.PropertyName).NotEmpty();
		RuleFor(command => command.PropertyValue).NotEmpty();
	}
}
