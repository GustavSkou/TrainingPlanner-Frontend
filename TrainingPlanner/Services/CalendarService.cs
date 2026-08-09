using System.Text.Json;
using TrainingPlanner.Models;
using TrainingPlanner.Services.Api;
using TrainingPlanner.Services.Contracts;

namespace TrainingPlanner.Services.Implementation;

public sealed class CalendarService(ITrainingPlannerApiClient apiClient) : ICalendarService
{
    private static readonly IReadOnlyList<TrainingTypeDTO> FallbackCategories =
    [
        new TrainingTypeDTO(1, "Running", "" ),
        new TrainingTypeDTO(2, "Cycling", "" ),
        new TrainingTypeDTO(3, "Swimming", "" ),
        new TrainingTypeDTO(4, "Workout", "")
    ];

    public Task<IReadOnlyList<TrainingPlanDTO>> GetTrainingPlansAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<TrainingTypeDTO>> GetTypesAsync(CancellationToken cancellationToken = default)
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