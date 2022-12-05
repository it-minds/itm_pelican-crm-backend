using System.Text.Json.Serialization;


namespace Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.Contact.Update;
public record UpdateContactCurrentProperties
{
	[JsonPropertyName("first_name")]
	public string? FirstName { get; set; }

	[JsonPropertyName("last_name")]
	public string? LastName { get; set; } = string.Empty;

	[JsonPropertyName("picture_128_url")]
	public string? PictureUrl { get; set; } = string.Empty;

	[JsonPropertyName("phone")]
	public List<ContactItem>? PhoneNumber { get; set; }

	[JsonPropertyName("email")]
	public List<ContactItem>? Email { get; set; }
}
