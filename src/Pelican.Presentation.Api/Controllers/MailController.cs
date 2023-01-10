using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Presentation.Api.Abstractions;

namespace Pelican.Presentation.Api.Controllers;
[Route("[controller]")]
public sealed class MailController : ApiController
{
	private readonly IMailService _mailService;
	public MailController(ISender sender, IMailService mailService) : base(sender)
	{
		_mailService = mailService;
	}

	//TODO: Re add when GetMailsQuery is created
	//[HttpGet]
	//public async Task<ActionResult<List<EmailDto>>> GetAllMails()
	//{
	//	return await Mediator.Send(new GetMailsQuery());
	//}


	//TODO: Re add when UpdateEmailCommand is created
	//[HttpPut("{id}")]
	//public async Task<ActionResult> UpdateEmail([FromRoute] int id, UpdateEmailCommand command)
	//{
	//	command.Id = id;
	//	await Mediator.Send(command);

	//	return NoContent();
	//}

	[HttpPost]
	public async Task<ActionResult> SendTestMail()
	{
		await _mailService.SendTestEmail();
		return NoContent();
	}
	//TODO: Re add after GeneratePreviewMailCommand is created
	//	[HttpPost("preview")]
	//	public async Task<ActionResult<string>> GeneratePreview([FromBody] GeneratePreviewMailCommand command)
	//	{
	//		return await Mediator.Send(command);
	//	}
}
