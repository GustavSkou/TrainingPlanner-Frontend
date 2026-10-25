using TrainingPlanner.Models;
using TrainingPlanner.Services.Api;
using TrainingPlanner.Services.Contracts;

namespace TrainingPlanner.Services.Implementation;

public sealed class AgendaService(ITrainingPlannerApiClient apiClient) : IAgendaService
{
    public async Task<TrainingPlanDTO> CreateTrainingPlanAsync(TrainingPlanDTO trainingPlanDTO, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(trainingPlanDTO);

        TrainingPlanDTO? result = await apiClient.PostAsync<TrainingPlanDTO, TrainingPlanDTO>("plans/create", trainingPlanDTO, cancellationToken);

        if (result is null)
        {
            throw new InvalidOperationException("The API did not return the created training plan.");
        }

        return result;
    }

    public Task<TrainingPlanDTO> UpdateTrainingPlanAsync(
        TrainingPlanDTO trainingPlanDTO,
        CancellationToken cancellationToken = default) =>
        UpdateAsync("plans", trainingPlanDTO.Id, trainingPlanDTO, "training plan", cancellationToken);

    public Task<WorkoutDTO> CreateWorkoutAsync(
        WorkoutDTO workoutDTO,
        CancellationToken cancellationToken = default) =>
        CreateAsync("workouts/create", workoutDTO, "workout", cancellationToken);

    public Task<WorkoutDTO> UpdateWorkoutAsync(
        WorkoutDTO workoutDTO,
        CancellationToken cancellationToken = default) =>
        UpdateAsync("workouts", workoutDTO.Id, workoutDTO, "workout", cancellationToken);

    public Task<IntervalDTO> CreateIntervalAsync(
        IntervalDTO intervalDTO,
        CancellationToken cancellationToken = default) =>
        CreateAsync("intervals/create", intervalDTO, "interval", cancellationToken);

    public Task<IntervalDTO> UpdateIntervalAsync(
        IntervalDTO intervalDTO,
        CancellationToken cancellationToken = default) =>
        UpdateAsync("intervals", intervalDTO.Id, intervalDTO, "interval", cancellationToken);

    public async Task<IReadOnlyList<TrainingPlanDTO>> GetTrainingPlansAsync(
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<TrainingPlanDTO>? plans =
            await apiClient.GetAsync<List<TrainingPlanDTO>>("plans", cancellationToken);

        return plans ?? Array.Empty<TrainingPlanDTO>();
    }

    public async Task<IReadOnlyList<TrainingTypeDTO>> GetTypesAsync(
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<TrainingTypeDTO>? types =
            await apiClient.GetAsync<List<TrainingTypeDTO>>("types", cancellationToken);

        return types ?? Array.Empty<TrainingTypeDTO>();
    }

    private async Task<T> CreateAsync<T>(
        string requestPath,
        T model,
        string modelName,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(model);

        T? result = await apiClient.PostAsync<T, T>(requestPath, model, cancellationToken);
        return result ?? throw new InvalidOperationException($"The API did not return the created {modelName}.");
    }

    private async Task<T> UpdateAsync<T>(
        string resourceName,
        int? id,
        T model,
        string modelName,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (!id.HasValue || id.Value <= 0)
        {
            throw new ArgumentException($"A valid {modelName} id is required for updates.", nameof(model));
        }

        T? result = await apiClient.PutAsync<T, T>($"{resourceName}/{id.Value}", model, cancellationToken);
        return result ?? throw new InvalidOperationException($"The API did not return the updated {modelName}.");
    }
}