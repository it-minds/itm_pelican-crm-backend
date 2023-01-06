using AutoMapper;

namespace Pelican.Application.AutoMapper;
public interface IAutoMap<T>
{
	void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
}
