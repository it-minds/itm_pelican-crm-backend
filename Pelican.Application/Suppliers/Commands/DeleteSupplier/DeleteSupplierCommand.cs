using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Suppliers.Commands.DeleteSupplier;

public sealed record DeleteSupplierCommand(
	long ObjectId) : ICommand;
