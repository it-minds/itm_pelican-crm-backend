namespace Pelican.Domain.Extensions;

public static class StringExtensions
{
	public static long? ToUnixTimeMillisecondsOrNull(this string stringDate)
		=> string.IsNullOrWhiteSpace(stringDate)
		? null
		: new DateTimeOffset(Convert.ToDateTime(stringDate)).ToUnixTimeMilliseconds();

	public static string? CheckAndShortenExceedingString(this string input, int maxLength)
		=> input.Length > maxLength
				? input.Substring(0, maxLength - 3) + ("...")
				: input;

}
