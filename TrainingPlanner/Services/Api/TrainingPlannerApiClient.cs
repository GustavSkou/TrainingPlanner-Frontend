using System.Net.Http.Json;

namespace TrainingPlanner.Services.Api;

public sealed class TrainingPlannerApiClient(HttpClient httpClient) : ITrainingPlannerApiClient
{
    public async Task<T?> GetAsync<T>(string requestUri, CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response = await httpClient.GetAsync(requestUri, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
    }

    public Task<T?> PostAsync<T>(string requestUri, T content, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}