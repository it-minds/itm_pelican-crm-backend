using System.Collections;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Pelican.Infrastructure.Persistence;
internal static class InMemoryForeignKeyValidator
{
	public static void ValidateForeignKeys(this DbContext context)
	{
		foreach (var entry in context.ChangeTracker.Entries().GroupBy(x => x.Metadata))
		{
			foreach (var fk in entry.Key.GetForeignKeys().Where(fk => fk.IsRequired))
			{
				fk.ValidateEntities(context, entry.Select(x => x.Entity));
			}
		}
	}

	static void ValidateEntities(this IForeignKey fk, DbContext context, IEnumerable<object> entities)
	{
		MethodInfo dbSetInfo = typeof(DbContext).GetMethod("Set", Type.EmptyTypes)
			.MakeGenericMethod(fk.PrincipalEntityType.ClrType);

		var isDict = fk.DeclaringEntityType.ClrType.IsAssignableTo(typeof(IDictionary));
		var propName = fk.Properties.First().Name;
		var property = fk.DeclaringEntityType.ClrType.GetProperty(propName);

		foreach (var ent in entities)
		{
			var findMethod = typeof(DbSet<>).MakeGenericType(fk.PrincipalEntityType.ClrType).GetMethod("Find");
			var set = dbSetInfo.Invoke(context, null);
			var valueToFind = isDict ? ((IDictionary)ent)[propName] : property.GetValue(ent);

			var res = findMethod.Invoke(set, new[] { new[] { valueToFind } });
			if (res == null)
				throw new DbUpdateException($"INSERT/UPDATE not meeting the constraint \"{fk.GetConstraintName()}\", on table \"{fk.PrincipalEntityType.GetTableName()}\", with column \"{fk.PrincipalKey.Properties.First().Name}\"");
		}
	}
}
