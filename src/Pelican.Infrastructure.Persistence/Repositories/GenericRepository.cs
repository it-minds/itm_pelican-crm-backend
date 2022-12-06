using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Pelican.Application.Abstractions.Data;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Domain.Primitives;

namespace Pelican.Infrastructure.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : Entity
{
	private readonly IPelicanContext _pelicanContext;

	public GenericRepository(IPelicanContext pelicanContext)
		=> _pelicanContext = pelicanContext
			?? throw new ArgumentNullException(nameof(pelicanContext));

	public IQueryable<T> FindAll()
		=> _pelicanContext
			.Set<T>()
			.AsNoTracking();

	public async Task<T?> GetByIdAsync(
		Guid id,
		CancellationToken cancellationToken = default)
	{
		if (id == Guid.Empty)
		{
			throw new ArgumentNullException(nameof(id));
		}

		return await _pelicanContext
			.Set<T>()
			.AsNoTracking()
			.FirstOrDefaultAsync(
				entity => entity.Id == id,
				cancellationToken);
	}

	public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
		=> _pelicanContext
			.Set<T>()
			.Where(expression);

	public async Task<T?> FirstOrDefaultAsync(
		Expression<Func<T, bool>> expression,
		CancellationToken cancellationToken = default)
		=> await _pelicanContext
			.Set<T>()
			.AsNoTracking()
			.FirstOrDefaultAsync(expression);

	public async Task<T> CreateAsync(
		T entity,
		CancellationToken cancellationToken = default)
	{
		if (entity is null)
		{
			throw new ArgumentNullException(nameof(entity));
		}

		await _pelicanContext
			.Set<T>()
			.AddAsync(entity, cancellationToken);

		return entity;
	}

	public async Task<IEnumerable<T>> CreateRangeAsync(
		IEnumerable<T> entities,
		CancellationToken cancellationToken = default)
	{
		if (entities is null)
		{
			throw new ArgumentNullException(nameof(entities));
		}

		await _pelicanContext
			.Set<T>()
			.AddRangeAsync(entities, cancellationToken);

		return entities;
	}

	public void Update(T entity) => _pelicanContext
		.Set<T>()
		.Update(entity);

	public void Delete(T entity) => _pelicanContext
		.Set<T>()
		.Remove(entity);

	public void AttachAsAdded(IEnumerable<T> entities)
	{
		_pelicanContext
			.Set<T>()
			.AttachRange(entities);

		foreach (var entity in entities)
		{
			_pelicanContext.Entry(entity).State = EntityState.Added;
		}
	}
}
