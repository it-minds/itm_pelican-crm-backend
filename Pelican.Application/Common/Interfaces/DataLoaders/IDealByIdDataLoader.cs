using GreenDonut;
using Pelican.Domain.Entities;

namespace Pelican.Application.Common.Interfaces.DataLoaders;
public interface IDealByIdDataLoader : IDataLoader<Guid, Deal>
{
}
