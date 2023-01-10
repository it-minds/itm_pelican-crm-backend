using Pelican.Application.AutoMapper;
using Pelican.Domain.Entities;

namespace Pelican.Application.Mail;
public class EmailDto : IAutoMap<Email>
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Subject { get; set; } = string.Empty;
	public string Heading1 { get; set; } = string.Empty;
	public string Paragraph1 { get; set; } = string.Empty;
	public string Heading2 { get; set; } = string.Empty;
	public string Paragraph2 { get; set; } = string.Empty;
	public string Heading3 { get; set; } = string.Empty;
	public string Paragraph3 { get; set; } = string.Empty;
	public string CtaButtonText { get; set; } = string.Empty;
}
