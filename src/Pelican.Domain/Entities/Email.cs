using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Email : Entity, ITimeTracked
{
	private string _name;
	private string _subject;
	private string _heading1;
	private string _heading2;
	private string _heading3;
	private string _paragraph1;
	private string _paragraph2;
	private string _paragraph3;
	private string _ctaButtonText;
	public Email() { }
	public string Name
	{
		get => _name;
		set
		{
			_name = value.Length > StringLengths.Name
					? value.Substring(0, StringLengths.Name - 3) + ("...")
					: value;
		}
	}
	public string Subject
	{
		get => _subject;
		set
		{
			_subject = value.Length > StringLengths.SubjectLine
					? value.Substring(0, StringLengths.SubjectLine - 3) + ("...")
					: value;
		}
	}
	public string Heading1
	{
		get => _heading1;
		set
		{
			_heading1 = value.Length > StringLengths.Heading
					? value.Substring(0, StringLengths.Heading - 3) + ("...")
					: value;
		}
	}
	public string Paragraph1
	{
		get => _paragraph1;
		set
		{
			_paragraph1 = value.Length > StringLengths.Paragraph
					? value.Substring(0, StringLengths.Paragraph - 3) + ("...")
					: value;
		}
	}
	public string Heading2
	{
		get => _heading2;
		set
		{
			_heading2 = value.Length > StringLengths.Heading
					? value.Substring(0, StringLengths.Heading - 3) + ("...")
					: value;
		}
	}
	public string Paragraph2
	{
		get => _paragraph2;
		set
		{
			_paragraph2 = value.Length > StringLengths.Paragraph
					? value.Substring(0, StringLengths.Paragraph - 3) + ("...")
					: value;
		}
	}
	public string Heading3
	{
		get => _heading3;
		set
		{
			_heading3 = value.Length > StringLengths.Heading
					? value.Substring(0, StringLengths.Heading - 3) + ("...")
					: value;
		}
	}
	public string Paragraph3
	{
		get => _paragraph3;
		set
		{
			_paragraph3 = value.Length > StringLengths.Paragraph
					? value.Substring(0, StringLengths.Paragraph - 3) + ("...")
					: value;
		}
	}
	public string CtaButtonText
	{
		get => _ctaButtonText;
		set
		{
			_ctaButtonText = value.Length > StringLengths.CtaButtonText
					? value.Substring(0, StringLengths.CtaButtonText - 3) + ("...")
					: value;
		}
	}
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }
}
