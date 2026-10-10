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

    public async Task<IReadOnlyList<TrainingPlanDTO>> GetTrainingPlansAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            IReadOnlyList<TrainingPlanDTO>? plans =
                await apiClient.GetAsync<List<TrainingPlanDTO>>("plans", cancellationToken);

            return plans ?? Array.Empty<TrainingPlanDTO>();
        }
        catch (HttpRequestException)
        {
            return CreateMockPlans();
        }
        catch (NotSupportedException)
        {
            return CreateMockPlans();
        }
        catch (JsonException)
        {
            return CreateMockPlans();
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

    private static IReadOnlyList<TrainingPlanDTO> CreateMockPlans()
    {
        DateTime today = DateTime.Today;
        return
        [
            CreateMockPlan(today, 1, "Easy morning run", 35, 5500),
            CreateMockPlan(today.AddDays(2), 2, "Steady ride", 55, 18000),
            CreateMockPlan(today.AddDays(4), 4, "Strength session", 45, 0),
            CreateMockPlan(today.AddDays(6), 1, "Long run", 70, 11000)
        ];
    }

    private static TrainingPlanDTO CreateMockPlan(
        DateTime date,
        int trainingTypeId,
        string name,
        int durationMinutes,
        int distanceMeters) =>
        new()
        {
            TrainingTypeId = trainingTypeId,
            Name = name,
            Date = date.Date,
            TrainingType = FallbackCategories.First(type => type.Id == trainingTypeId),
            Workout = new WorkoutDTO
            {
                Name = name,
                DurationMinutes = durationMinutes,
                DistanceMeters = distanceMeters
            }
        };
}