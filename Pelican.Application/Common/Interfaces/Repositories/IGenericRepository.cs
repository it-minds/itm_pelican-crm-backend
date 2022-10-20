using System.Linq.Expressions;

namespace Pelican.Application.Common.Interfaces.Repositories;
public interface IGenericRepository<T>
{
	IQueryable<T> FindAll();
	Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
	IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
	Task<T> CreateAsync(T entity, CancellationToken cancellationToken);
	Task<IEnumerable<T>> CreateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
	void Update(T entity);
	void Delete(T entity);
}
