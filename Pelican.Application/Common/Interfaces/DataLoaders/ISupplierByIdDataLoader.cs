using GreenDonut;
using Pelican.Domain.Entities;

namespace Pelican.Application.Common.Interfaces.DataLoaders;
public interface ISupplierByIdDataLoader : IDataLoader<Guid, Supplier>
{
}
