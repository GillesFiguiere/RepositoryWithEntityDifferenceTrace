using System.Diagnostics;
using System.Reflection;
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
        var methodName = MethodBase.GetCurrentMethod()?.Name;
        _logger.Log(
            LogLevel.Information,
            "Début de {Class}.{Method}<{Type}>",
            GetType().Name,
            methodName,
            entity.GetType().Name);

        var initialEntity = Get(entity.Id);
        if (initialEntity != null) Log(entity, initialEntity);
        
        return CreateOrUpdateAndTraceExecutionDuration(entity, methodName);
    }

    private int CreateOrUpdateAndTraceExecutionDuration(T entity, string? methodName)
    {
        var numberModified = 0;
        
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        try
        {
            numberModified = _repository.CreateOrUpdate(entity);
        }
        finally
        {
            stopWatch.Stop();
            _logger.Log(
                LogLevel.Information,
                "Fin de {Class}.{Method}<{Type}>, Durée {Duration} ms",
                GetType().Name,
                methodName,
                entity.GetType().Name,
                stopWatch.ElapsedMilliseconds
            );
        }

        return numberModified;
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