using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Suppliers.Commands.UpdateSupplier;
public sealed record UpdateSupplierCommand(
	long ObjectId,
	string UserId,
	string PropertyName,
	string PropertyValue) : ICommand;
