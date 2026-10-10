using TrainingPlanner.Models;

namespace TrainingPlanner.Services.Contracts;

public interface IAgendaService
{
    Task<IReadOnlyList<TrainingTypeDTO>> GetTypesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TrainingPlanDTO>> GetTrainingPlansAsync(CancellationToken cancellationToken = default);

    Task<TrainingPlanDTO> CreateTrainingPlanAsync(TrainingPlanDTO trainingPlanDTO, CancellationToken cancellationToken = default);
    Task<TrainingPlanDTO> UpdateTrainingPlanAsync(TrainingPlanDTO trainingPlanDTO, CancellationToken cancellationToken = default);
    Task<WorkoutDTO> CreateWorkoutAsync(WorkoutDTO workoutDTO, CancellationToken cancellationToken = default);
    Task<WorkoutDTO> UpdateWorkoutAsync(WorkoutDTO workoutDTO, CancellationToken cancellationToken = default);
    Task<IntervalDTO> CreateIntervalAsync(IntervalDTO intervalDTO, CancellationToken cancellationToken = default);
    Task<IntervalDTO> UpdateIntervalAsync(IntervalDTO intervalDTO, CancellationToken cancellationToken = default);
}