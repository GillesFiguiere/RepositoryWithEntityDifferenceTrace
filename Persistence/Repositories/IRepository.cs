using Test.Persistence.Entities;

namespace Test.Persistence.Repositories;

public interface IRepository<T> where T : Entity
{
    T? Get(Guid id);
    int CreateOrUpdate(T entity);
}