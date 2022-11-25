using GreenDonut;

namespace Pelican.Application.Abstractions.Data.DataLoaders;

public interface IGenericDataLoader<T> : IDataLoader<Guid, T>
{
	//Intentionally Empty
}
