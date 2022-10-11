using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Pelican.Application.Common.Interfaces;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Primitives;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class GenericRepository<T> : IGenericRepository<T> where T : Entity
{
	//This Repository contains base functions that will be inherited by all specific repositories
	protected PelicanContext PelicanContext { get; set; }
	public GenericRepository(IPelicanContext pelicanContext) =>
		PelicanContext = (PelicanContext)pelicanContext ?? throw new ArgumentNullException(nameof(PelicanContext));

	public IQueryable<T> FindAll() => PelicanContext.Set<T>().AsNoTracking();

	public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) => PelicanContext.Set<T>().Where(expression).AsNoTracking();
	public void Create(T entity) => PelicanContext.Set<T>().Add(entity);
	public void CreateRange(IEnumerable<T> entities) => PelicanContext.Set<T>().AddRange(entities);
	public void Update(T entity) => PelicanContext.Set<T>().Update(entity);
	public void Delete(T entity) => PelicanContext.Set<T>().Remove(entity);
}
