using System.Reflection;

namespace Pelican.Infrastructure.HubSpot;
public static class AssemblyReference
{
	public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
