using GreenDonut;

namespace Pelican.Application.Common.Interfaces.DataLoaders;
public interface IGenericDataLoader<T> : IDataLoader<Guid, T>
{
}
