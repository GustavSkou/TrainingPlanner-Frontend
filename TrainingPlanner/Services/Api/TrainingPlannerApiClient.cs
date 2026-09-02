using System.Net.Http.Json;

namespace TrainingPlanner.Services.Api;

public sealed class TrainingPlannerApiClient(HttpClient httpClient) : ITrainingPlannerApiClient
{
    public async Task<T?> GetAsync<T>(string requestPath, CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response = await httpClient.GetAsync(requestPath, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string requestPath, TRequest content, CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response = await httpClient.PostAsJsonAsync(requestPath, content, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }
}