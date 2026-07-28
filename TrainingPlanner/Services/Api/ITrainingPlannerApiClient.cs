namespace TrainingPlanner.Services.Api;

public interface ITrainingPlannerApiClient
{
    Task<T?> GetAsync<T>(string requestUri, CancellationToken cancellationToken = default);
}