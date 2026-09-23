using TrainingPlanner.Models;
using TrainingPlanner.Services.Api;
using TrainingPlanner.Services.Contracts;

namespace TrainingPlanner.Services.Implementation;

public sealed class AgendaService(ITrainingPlannerApiClient apiClient) : IAgendaService
{
    public async Task<TrainingPlanDTO> CreateTrainingPlanAsync(
        TrainingPlanDTO trainingPlanDTO,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(trainingPlanDTO);

        TrainingPlanDTO? result = await apiClient.PostAsync<TrainingPlanDTO, TrainingPlanDTO>(
            "plans/create",
            trainingPlanDTO,
            cancellationToken);

        if (result is null)
        {
            throw new InvalidOperationException("The API did not return the created training plan.");
        }

        return result;
    }

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
}