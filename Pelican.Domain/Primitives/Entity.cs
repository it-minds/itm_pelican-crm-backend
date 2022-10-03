namespace Pelican.Domain.Primitives;
public abstract class Entity<TKey>
{
	public TKey Id { get; init; }
}
