namespace Pelican.Application.BogusExtension;
public static class BogusExtensions
{
	public static T? OrNull<T>(this T? value)
	   where T : struct
	{
		return value.GetHashCode() % 2 == 0 ? value : null;
	}

	public static T? OrNull<T>(this T value)
	   where T : struct
	{
		return value.GetHashCode() % 2 == 0 ? (T?)value : null;
	}
}
