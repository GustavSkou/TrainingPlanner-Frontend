using TrainingPlanner.Models;

namespace TrainingPlanner.Services.Contracts;

public interface IAgendaService
{
    Task<IReadOnlyList<TrainingTypeDTO>> GetTypesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TrainingPlanDTO>> GetTrainingPlansAsync(CancellationToken cancellationToken = default);

    Task<TrainingPlanDTO> CreateTrainingPlanAsync(TrainingPlanDTO trainingPlanDTO, CancellationToken cancellationToken = default);
}