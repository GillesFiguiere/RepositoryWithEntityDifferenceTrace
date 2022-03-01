using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Test.Persistence.Entities;

namespace Test.Persistence.Repositories;

public class RepositoryWithEntityDifferenceTrace<T> : IRepository<T> where T : Entity
{
    private readonly IRepository<T> _repository;
    private readonly ILogger _logger;

    public RepositoryWithEntityDifferenceTrace(IRepository<T> repository, ILogger logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public T? Get(Guid id) => _repository.Get(id);

    public int CreateOrUpdate(T entity)
    {
        var initialEntity = Get(entity.Id);
        if (initialEntity != null) Log(entity, initialEntity);

        return _repository.CreateOrUpdate(entity);
    }

    private void Log(T entity, T? initialEntity) =>
        _logger.Log(
            LogLevel.Debug,
            "Mise à jour de objet de type {Type} avec l'id {Id}, valeur avant {InitialEntity}, valeur après {UpdatedEntity}",
            entity.GetType().Name,
            entity.Id,
            JsonConvert.SerializeObject(initialEntity),
            JsonConvert.SerializeObject(entity));
}