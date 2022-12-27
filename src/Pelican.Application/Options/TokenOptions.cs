namespace Pelican.Application.Options;
public class TokenOptions
{
	public static string Tokens { get; set; } = "Tokens";
	public string Secret { get; set; } = "VERY_SECRET_SECRET";
	public double ExpireHours { get; set; }
	public double SsoExpireDays { get; set; }
}
