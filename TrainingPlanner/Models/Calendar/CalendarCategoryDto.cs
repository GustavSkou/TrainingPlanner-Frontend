namespace TrainingPlanner.Models.Calendar;

public sealed record TrainingTypeDto(
    int Id,
    string Name,
    string? Description,
    bool IsSelected = true);