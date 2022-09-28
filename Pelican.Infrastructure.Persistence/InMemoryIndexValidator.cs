using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Pelican.Infrastructure.Persistence;
internal static class InMemoryIndexValidator
{
	public static void ValidateIndexes(this DbContext context)
	{
		foreach (var entry in context.ChangeTracker.Entries().GroupBy(x => x.Metadata))
		{
			foreach (var index in entry.Key.UniqueIndices())
			{
				index.ValidateEntities(entry.Select(x => x.Entity));
			}
		}
	}

	static void ValidateEntities(this IIndex index, IEnumerable<object> entities)
	{
		var dictionary = new Dictionary<int, List<object>>();
		foreach (var entity in entities)
		{
			var valueLookup = index.GetProperties(entity).ToList();
			var values = valueLookup.Select(x => x.value).ToList();
			var hash = values.GetHash();

			if (!dictionary.ContainsKey(hash) || hash == 0)
			{
				dictionary[hash] = values;
				continue;
			}

			var builder = new StringBuilder($"Conflicting values for unique index. Entity: {entity.GetType().FullName},\r\nIndex Properties:\r\n");
			foreach (var (name, value) in valueLookup)
			{
				builder.AppendLine($"    {name}='{value}'");
			}
			throw new Exception(builder.ToString());
		}
	}

	static IEnumerable<IIndex> UniqueIndices(this IEntityType entityType)
	{
		return entityType.GetIndexes()
			.Where(x => x.IsUnique);
	}

	static int GetHash(this IEnumerable<object> values)
	{
		return values.Where(x => x != null)
			.Sum(x => x.GetHashCode());
	}

	static IEnumerable<(string name, object value)> GetProperties(this IIndex index, object entity)
	{
		return index.Properties
			.Select(property => property.PropertyInfo)
			.Select(info => (info.Name, info.GetValue(entity)));
	}
}
