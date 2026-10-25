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

    public async Task<IReadOnlyList<TrainingPlanDTO>> GetTrainingPlansAsync(int? userId, CancellationToken cancellationToken = default)
    {
        
        try
        {
            string requestUri = $"plans/user?userId={userId}";
            TrainingPlanDTO[]? plans =
                await apiClient.GetAsync<TrainingPlanDTO[]>(requestUri, cancellationToken);
            return plans is null ? Array.Empty<TrainingPlanDTO>() : plans;
        }
        catch (HttpRequestException)
        {
            return [];
        }
        catch (JsonException)
        {
            
            string requestUri = $"plans/user?userId={userId}";
            TrainingPlanDTO[]? plans = [await apiClient.GetAsync<TrainingPlanDTO>(requestUri, cancellationToken)];
            return plans is null ? Array.Empty<TrainingPlanDTO>() : plans;
        }
    }

    public async Task<IReadOnlyList<TrainingTypeDTO>> GetTypesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            IReadOnlyList<TrainingTypeDTO>? categories =
                await apiClient.GetAsync<List<TrainingTypeDTO>>("types", cancellationToken);
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