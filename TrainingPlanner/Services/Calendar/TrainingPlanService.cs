using TrainingPlanner.Models;

namespace TrainingPlanner.Services.Calendar;

public interface ITrainingPlanService
{
    Task<IReadOnlyList<TrainingTypeDTO>> GetCategoriesAsync(CancellationToken cancellationToken = default);
}