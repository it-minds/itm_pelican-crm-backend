using Pelican.Domain.Entities;

namespace Pelican.Application.Options;
public class MailOptions
{
	public const string MailSettings = "MailSettings";
	public string ApiKey { get; set; } = string.Empty;
	public string Mail { get; set; } = string.Empty;
	public string DisplayName { get; set; } = string.Empty;
	public string UserName { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public string Host { get; set; } = string.Empty;
	public string baseUrl { get; set; } = string.Empty;
	public List<Email> DefaultEmails { get; set; } = new List<Email>();
}
