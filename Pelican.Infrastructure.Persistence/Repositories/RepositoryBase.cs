using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Repositories;

namespace Pelican.Infrastructure.Persistence.Repositories;
public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
	protected PelicanContext PelicanContext { get; set; }
	public RepositoryBase(IPelicanContext pelicanContext)
	{
		PelicanContext = (PelicanContext)pelicanContext;
	}

	public IQueryable<T> FindAll() => PelicanContext.Set<T>().AsNoTracking();

	public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) => PelicanContext.Set<T>().Where(expression).AsNoTracking();
	public void Create(T entity) => PelicanContext.Set<T>().Add(entity);
	public void Update(T entity) => PelicanContext.Set<T>().Update(entity);
	public void Delete(T entity) => PelicanContext.Set<T>().Remove(entity);
}
