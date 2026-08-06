using System.Text.Json;
using TrainingPlanner.Models;
using TrainingPlanner.Services.Api;

namespace TrainingPlanner.Services.Calendar;

public sealed class CalendarSidebarService(ITrainingPlannerApiClient apiClient) : ICalendarSidebarService
{
    private static readonly IReadOnlyList<TrainingTypeDTO> FallbackCategories =
    [
        new TrainingTypeDTO(1, "Running", "" ),
        new TrainingTypeDTO(2, "Cycling", "" ),
        new TrainingTypeDTO(3, "Swimming", "" ),
        new TrainingTypeDTO(4, "Workout", "")
    ];

    public async Task<IReadOnlyList<TrainingTypeDTO>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            IReadOnlyList<TrainingTypeDTO>? categories = await apiClient.GetAsync<List<TrainingTypeDTO>>("http://localhost:5001/types", cancellationToken);
            return categories ?? FallbackCategories;
        }
        catch (HttpRequestException)
        {
            return FallbackCategories;
        }
        catch (NotSupportedException)
        {
            return FallbackCategories;
        }
        catch (JsonException)
        {
            return FallbackCategories;
        }
    }
}