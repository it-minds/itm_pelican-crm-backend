using System.Reflection;
using AutoMapper;

namespace Pelican.Application.AutoMapper;
public class MappingProfile : Profile
{
	public MappingProfile()
	{
		ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
	}

	private void ApplyMappingsFromAssembly(Assembly assembly)
	{
		var types = assembly.GetExportedTypes()
			.Where(t => t.GetInterfaces().Any(i =>
				i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAutoMap<>)))
			.ToList();

		foreach (var type in types)
		{
			var instance = Activator.CreateInstance(type);

			var methodInfo = type.GetMethod("Mapping")
							 ?? type.GetInterface("IAutoMap`1").GetMethod("Mapping");

			methodInfo?.Invoke(instance, new object[] { this });
		}
	}
}
