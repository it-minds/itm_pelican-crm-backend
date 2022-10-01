using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Pelican.Presentation.Api.Abstractions;

public class ApiController : ControllerBase
{
	protected readonly ISender Sender;

	protected ApiController(ISender sender)
	{
		Sender = sender;
	}
}
