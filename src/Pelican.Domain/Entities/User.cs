using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class User : Entity, ITimeTracked
{
	private string _name;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public User() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
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
	//TODO re add this line when RoleEnum has been added
	//public new RoleEnum Role { get; set; } = RoleEnum.User
	public string SSOTokenId { get; set; } = string.Empty;
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }
}
