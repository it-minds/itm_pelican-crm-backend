using MediatR;
using Pelican.Application.Authentication;
using Pelican.Application.Users.Commands.CreateAdmin;
using Pelican.Application.Users.Commands.CreateStandardUser;
using Pelican.Application.Users.Commands.UpdatePassword;
using Pelican.Application.Users.Commands.UpdateUser;
using Pelican.Domain.Shared;

namespace Pelican.Presentation.GraphQL.Users;

[ExtendObjectType(OperationTypeNames.Mutation)]
public sealed class UsersMutation
{
	public async Task<ResultDto<UserDto>> CreateStandardUser(
		string name,
		string email,
		string password,
		[Service] IMediator mediator,
		CancellationToken cancellationToken)
		=> await mediator.Send(
			new CreateStandardUserCommand(name, email, password),
			cancellationToken);

	public async Task<ResultDto<UserDto>> CreateAdmin(
		string name,
		string email,
		string password,
		[Service] IMediator mediator,
		CancellationToken cancellationToken)
		=> await mediator.Send(
				new CreateAdminCommand(name, email, password),
				cancellationToken);

	public async Task<ResultDto<UserDto>> UpdateUser(
		UserDto user,
		[Service] IMediator mediator,
		CancellationToken cancellationToken)
		=> await mediator.Send(
			new UpdateUserCommand(user),
			cancellationToken);

	public async Task<ResultDto<UserDto>> UpdatePassword(
		string password,
		[Service] IMediator mediator,
		CancellationToken cancellationToken)
		=> await mediator.Send(
			new UpdatePasswordCommand(password),
			cancellationToken);
}
