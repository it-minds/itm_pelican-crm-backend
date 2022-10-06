using GreenDonut;
using Pelican.Domain.Entities;

namespace Pelican.Application.Common.Interfaces.DataLoaders;
public interface IClientByIdDataLoader : IDataLoader<Guid, Client>
{
}
