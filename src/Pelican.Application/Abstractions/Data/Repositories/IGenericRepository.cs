using System.Linq.Expressions;

namespace Pelican.Application.Abstractions.Data.Repositories;
public interface IGenericRepository<T>
{
	IQueryable<T> FindAll();
	Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
	IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
	Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
	T Attach(T entity);
	Task<IEnumerable<T>> CreateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
	Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
	void Update(T entity);
	void Delete(T entity);
	void AttachAsAdded(IEnumerable<T> entities);
}
