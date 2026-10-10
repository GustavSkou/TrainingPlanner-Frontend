namespace TrainingPlanner.Services.Api;

public interface ITrainingPlannerApiClient
{
    Task<T?> GetAsync<T>(string requestUri, CancellationToken cancellationToken = default);
    Task<TResponse?> PostAsync<TRequest, TResponse>(string requestUri, TRequest content, CancellationToken cancellationToken = default);
    Task<TResponse?> PutAsync<TRequest, TResponse>(string requestUri, TRequest content, CancellationToken cancellationToken = default);
    Task DeleteAsync(string requestUri, CancellationToken cancellationToken = default);
}