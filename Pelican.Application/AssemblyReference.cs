using System.Reflection;

namespace Pelican.Application;
public static class AssemblyReference
{
	public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
