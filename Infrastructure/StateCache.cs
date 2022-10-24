using Microsoft.Extensions.Caching.Memory;
using Test_task.Infrastructure.Models;

namespace Test_task.Infrastructure;

public class StateCache : IStateCache
{
    private MemoryCache _cache = new(new MemoryCacheOptions());

    public ConvertingState AddCache(ConvertingState state)
    {
        object id = state.Id;
        
        if (_cache.TryGetValue(id, out _)) 
            _cache.Remove(id);
        
        var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1));
        _cache.Set(id, state, cacheEntryOptions);
        
        return state;
    }

    public ConvertingState GetCache(Guid stateId)
    {
        ConvertingState cacheEntry;
        object id = stateId;
        return _cache.TryGetValue(id, out cacheEntry) ? cacheEntry : cacheEntry;
    }
}