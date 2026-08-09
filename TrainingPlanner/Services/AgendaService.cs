using System.Text.Json;
using TrainingPlanner.Models;
using TrainingPlanner.Services.Api;
using TrainingPlanner.Services.Contracts;

namespace TrainingPlanner.Services.Implementation;

public sealed class AgendaService(ITrainingPlannerApiClient apiClient) : IAgendaService
{
    public Task CreateTrainingPlanAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<TrainingPlanDTO>> GetTrainingPlansAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<TrainingTypeDTO>> GetTypesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}