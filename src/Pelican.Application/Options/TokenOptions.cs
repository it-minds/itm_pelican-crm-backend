namespace Pelican.Application.Options;
public class TokenOptions
{
	public const string Tokens = "Tokens";
	public string Secret = "VERY_SECRET_SECRET";
	public double ExpireHours { get; set; }
	public double SsoExpireDays { get; set; }
}
