namespace Pelican.Application.Extensions;

public static class StringExtensions
{
	public static long? ToUnixTimeMillisecondsOrNull(this string stringDate)
		=> string.IsNullOrWhiteSpace(stringDate)
		? null
		: new DateTimeOffset(Convert.ToDateTime(stringDate)).ToUnixTimeMilliseconds();
}
