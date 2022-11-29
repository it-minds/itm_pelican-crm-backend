using FluentValidation;

namespace Pelican.Application.Contacts.Commands.UpdateContact;

<<<<<<< HEAD:src/Pelican.Application/Contacts/Commands/UpdateContact/UpdateContactCommandValidator.cs
internal sealed class UpdateContactCommandValidator : AbstractValidator<UpdateContactCommand>
{
	public UpdateContactCommandValidator()
=======
internal sealed class UpdateDealHubSpotCommandValidator : AbstractValidator<UpdateDealHubSpotCommand>
{
	public UpdateDealHubSpotCommandValidator()
>>>>>>> Added enpoint for update Pipedrive deal:src/Pelican.Application/Deals/HubSpotCommands/UpdateDeal/UpdateDealHubSpotCommandValidator.cs
	{
		RuleFor(command => command.ObjectId).NotEmpty();
		RuleFor(command => command.SupplierHubSpotId).NotEmpty();
		RuleFor(command => command.PropertyName).NotEmpty();
		RuleFor(command => command.PropertyValue).NotEmpty();
	}
}
