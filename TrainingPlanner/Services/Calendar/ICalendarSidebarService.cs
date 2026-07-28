using TrainingPlanner.Models.Calendar;

namespace TrainingPlanner.Services.Calendar;

public interface ICalendarSidebarService
{
    Task<IReadOnlyList<TrainingTypeDto>> GetCategoriesAsync(CancellationToken cancellationToken = default);
}