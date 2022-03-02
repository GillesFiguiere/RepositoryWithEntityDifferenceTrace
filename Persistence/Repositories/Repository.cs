using Test.Persistence.Entities;

namespace Test.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T: Entity
{
    private readonly Context _context;

    public Repository(Context context) =>
        _context = context;

    public T? Get(Guid id) => _context.Find<T>(id);

    public int CreateOrUpdate(T entity)
    {
        var existingEntity = _context.Find<T>(entity.Id);
        
        if (existingEntity == null)
            _context.Add(entity);
        else
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
        
        return _context.SaveChanges();
    }
}