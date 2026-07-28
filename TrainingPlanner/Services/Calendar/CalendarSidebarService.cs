using System.Text.Json;
using TrainingPlanner.Models.Calendar;
using TrainingPlanner.Services.Api;

namespace TrainingPlanner.Services.Calendar;

public sealed class CalendarSidebarService(ITrainingPlannerApiClient apiClient) : ICalendarSidebarService
{
    private static readonly IReadOnlyList<TrainingTypeDto> FallbackCategories =
    [
        new TrainingTypeDto(1, "Running", "" ),
        new TrainingTypeDto(2, "Cycling", "" ),
        new TrainingTypeDto(3, "Swimming", "" ),
        new TrainingTypeDto(4, "Workout", "")
    ];

    public async Task<IReadOnlyList<TrainingTypeDto>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            IReadOnlyList<TrainingTypeDto>? categories = await apiClient.GetAsync<List<TrainingTypeDto>>("http://localhost:5001/types", cancellationToken);
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