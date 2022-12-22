/// <summary>
/// Specifies the class this attribute is applied to requires authentication.
/// </summary>
namespace Pelican.Application.Security;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class AuthenticatedAttribute : Attribute
{
	/// <summary>
	/// Initializes a new instance of the <see cref="AuthenticatedAttribute"/> class.
	/// </summary>
	public AuthenticatedAttribute() { }
}
