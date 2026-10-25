using System.Net.Http.Json;
using System.Text.Json;

namespace TrainingPlanner.Services.Api;

public sealed class TrainingPlannerApiClient(HttpClient httpClient) : ITrainingPlannerApiClient
{
    public async Task<T?> GetAsync<T>(string requestPath, CancellationToken cancellationToken = default)
    {
        /*using HttpResponseMessage response = await httpClient.GetAsync(requestPath, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
*/
        using HttpResponseMessage response =
            await httpClient.GetAsync(requestPath, cancellationToken);

        string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        Console.WriteLine($"GET {requestPath}");
        Console.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode}");
        Console.WriteLine($"Content-Type: {response.Content.Headers.ContentType}");
        Console.WriteLine($"Body: {responseBody}");

        response.EnsureSuccessStatusCode();

        return JsonSerializer.Deserialize<T>(
            responseBody,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string requestPath, TRequest content, CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response = await httpClient.PostAsJsonAsync(requestPath, content, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string requestPath, TRequest content, CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response = await httpClient.PutAsJsonAsync(requestPath, content, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(string requestPath, CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response = await httpClient.DeleteAsync(requestPath, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}