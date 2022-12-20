namespace Pelican.Domain.Entities;
public abstract class User : Entity, ITimeTracked
{
	private string _name = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
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
	public RoleEnum Role { get; set; }
	public string SSOTokenId { get; set; } = string.Empty;
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }
}
