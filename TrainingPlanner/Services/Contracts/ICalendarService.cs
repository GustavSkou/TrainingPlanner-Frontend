using TrainingPlanner.Models;

namespace TrainingPlanner.Services.Contracts;

public interface ICalendarService
{
    Task<IReadOnlyList<TrainingTypeDTO>> GetTypesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TrainingPlanDTO>> GetTrainingPlansAsync(int? userId, CancellationToken cancellationToken = default);
}