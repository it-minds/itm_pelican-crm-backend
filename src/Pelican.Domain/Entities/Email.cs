using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Email : Entity, ITimeTracked
{
	public string Name { get; set; } = string.Empty;
	public string Subject { get; set; } = string.Empty;
	public string Heading1 { get; set; } = string.Empty;
	public string Paragraph1 { get; set; } = string.Empty;
	public string Heading2 { get; set; } = string.Empty;
	public string Paragraph2 { get; set; } = string.Empty;
	public string Heading3 { get; set; } = string.Empty;
	public string Paragraph3 { get; set; } = string.Empty;
	public string CtaButtonText { get; set; } = string.Empty;
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }
}
