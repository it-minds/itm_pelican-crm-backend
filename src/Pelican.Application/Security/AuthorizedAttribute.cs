namespace Pelican.Application.Security;
/// <summary>
/// Specifies the class this attribute is applied to requires authorization.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class AuthorizedAttribute : Attribute
{
	/// <summary>
	/// Initializes a new instance of the <see cref="AuthorizedAttribute"/> class. 
	/// </summary>
	public AuthorizedAttribute() { }

	/// <summary>
	/// Gets or sets a role that are allowed to access the resource.
	/// </summary>
	public RoleEnum Role { get; set; }
}
