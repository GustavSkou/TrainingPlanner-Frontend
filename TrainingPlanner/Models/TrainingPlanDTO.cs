using System;

namespace TrainingPlanner.Models
{
    public class TrainingPlanDTO
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public int TrainingTypeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public TrainingTypeDTO? TrainingType { get; set; }
        public WorkoutDTO Workout { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
