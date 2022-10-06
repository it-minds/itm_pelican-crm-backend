using System.Reflection;
using HotChocolate.Types.Descriptors;
using Pelican.Infrastructure.Persistence;

namespace Pelican.Presentation.GraphQL.Extensions;
public class UseApplicationDbContextAttribute : ObjectFieldDescriptorAttribute
{
	public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
	{
		descriptor.UseDbContext<IDbContext>();
	}
}
