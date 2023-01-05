using Pelican.Domain.Extensions;
using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Email : Entity, ITimeTracked
{
	private string _name = string.Empty;
	private string _subject = string.Empty;
	private string _heading1 = string.Empty;
	private string _heading2 = string.Empty;
	private string _heading3 = string.Empty;
	private string _paragraph1 = string.Empty;
	private string _paragraph2 = string.Empty;
	private string _paragraph3 = string.Empty;
	private string _ctaButtonText = string.Empty;
	public Email() { }
	public string Name
	{
		get => _name;
		set
		{
			_name = value.CheckAndShortenExceedingString(StringLengths.Name);
		}
	}
	public string Subject
	{
		get => _subject;
		set
		{
			_subject = value.CheckAndShortenExceedingString(StringLengths.SubjectLine);
		}
	}
	public string Heading1
	{
		get => _heading1;
		set
		{
			_heading1 = value.CheckAndShortenExceedingString(StringLengths.Heading);
		}
	}
	public string Paragraph1
	{
		get => _paragraph1;
		set
		{
			_paragraph1 = value.CheckAndShortenExceedingString(StringLengths.Paragraph);
		}
	}
	public string Heading2
	{
		get => _heading2;
		set
		{
			_heading2 = value.CheckAndShortenExceedingString(StringLengths.Heading);
		}
	}
	public string Paragraph2
	{
		get => _paragraph2;
		set
		{
			_paragraph2 = value.CheckAndShortenExceedingString(StringLengths.Paragraph);
		}
	}
	public string Heading3
	{
		get => _heading3;
		set
		{
			_heading3 = value.CheckAndShortenExceedingString(StringLengths.Heading);
		}
	}
	public string Paragraph3
	{
		get => _paragraph3;
		set
		{
			_paragraph3 = value.CheckAndShortenExceedingString(StringLengths.Paragraph);
		}
	}
	public string CtaButtonText
	{
		get => _ctaButtonText;
		set
		{
			_ctaButtonText = value.CheckAndShortenExceedingString(StringLengths.CtaButtonText);
		}
	}
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }
}
