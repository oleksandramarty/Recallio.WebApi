﻿using System.Linq.Expressions;

namespace Recallio.Interfaces;

public interface IReadGenericService<T> where T : class
{
    Task<IList<T>> GetAllAsync(CancellationToken cancellationToken);
    Task<IList<T>> GetWithPaginationAsync(int page, int take, CancellationToken cancellationToken);
    Task<T> GetByIdAsync(Guid id,  CancellationToken cancellationToken);
    Task<T> GetByPropertyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    Task<IList<T>> GetListByPropertyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
}
