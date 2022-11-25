namespace Pelican.Domain.Primitives;
public interface ITimeTracked
{
	long CreatedAt { get; set; }
	long? LastUpdatedAt { get; set; }
}
