using Microsoft.AspNetCore.Http;

namespace Pelican.Application.Mail;
public class MailRequestDto
{
	public string ToEmail { get; set; } = string.Empty;
	public string ToName { get; set; } = string.Empty;
	public string Subject { get; set; } = string.Empty;
	public string Body { get; set; } = string.Empty;
	public List<IFormFile> Attachments { get; set; } = new List<IFormFile>();
}
