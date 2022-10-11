﻿using System.Linq.Expressions;

namespace Pelican.Domain.Repositories;
public interface IGenericRepository<T>
{
	IQueryable<T> FindAll();
	IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
	void Create(T entity);
	void CreateRange(IEnumerable<T> entities);
	void Update(T entity);
	void Delete(T entity);
}
