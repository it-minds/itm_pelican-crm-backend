using MediatR;
using Pelican.Application.Authentication.CreateStandardUser;
using Pelican.Application.Users.Commands.CreateAdmin;
using Pelican.Application.Users.Commands.UpdatePassword;
using Pelican.Application.Users.Commands.UpdateUser;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Presentation.GraphQL.Users;

[ExtendObjectType(OperationTypeNames.Mutation)]
public sealed class UsersMutation
{
	public async Task<Result> CreateStandardUser(
		string name,
		string email,
		string password,
		[Service] IMediator mediator,
		CancellationToken cancellationToken)
		=> await mediator.Send(
			new CreateStandardUserCommand(name, email, password),
			cancellationToken);

	public async Task<Result> CreateAdmin(
		string name,
		string email,
		string password,
		[Service] IMediator mediator,
		CancellationToken cancellationToken)
		=> await mediator.Send(
			new CreateAdminCommand(name, email, password),
			cancellationToken);

	public async Task<Domain.Shared.Result<User>> UpdateUser(
		User user,
		[Service] IMediator mediator,
		CancellationToken cancellationToken)
		=> await mediator.Send(
			new UpdateUserCommand(user),
			cancellationToken);

	public async Task<Domain.Shared.Result<User>> UpdatePassword(
		string password,
		[Service] IMediator mediator,
		CancellationToken cancellationToken)
		=> await mediator.Send(
			new UpdatePasswordCommand(password),
			cancellationToken);
}
