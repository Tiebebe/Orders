namespace Application.Contracts;

public interface ICacheService
{
    bool TryGet<T>(string cacheKey, out T? value);
    void Set<T>(string cacheKey, T value, TimeSpan timeSpan);
    void Remove(string cacheKey);
}
