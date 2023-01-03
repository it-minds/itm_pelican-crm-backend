using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pelican.Application.Authentication;
using Pelican.Application.Authentication.CheckAuth;
using Pelican.Application.Authentication.Login;
using Pelican.Presentation.Api.Abstractions;

namespace Pelican.Presentation.Api.Controllers;

[Route("[controller]")]
public sealed class AuthController : ApiController
{
	public AuthController(ISender sender)
	: base(sender)
	{ }

	[HttpPost]
	public async Task<ActionResult<UserTokenDto>> Login(string email, string password)
	{
		var result = await Sender.Send(new LoginCommand(email, password));

		return result.IsSuccess
		? result.Value
		: BadRequest(result.Error);
	}

	[HttpPut]
	public async Task<ActionResult<UserDto>> CheckAuth()
	{
		var result = await Sender.Send(new CheckAuthCommand());

		return result.IsSuccess
		? result.Value
		: BadRequest(result.Error);
	}
}
