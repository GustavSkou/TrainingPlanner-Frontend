using System;

namespace TrainingPlanner.Models
{
    public class WorkoutDTO
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public int TrainingPlanId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DurationMinutes { get; set; }
        public int DistanceMeters { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<SegmentDTO> Segments { get; set; } = new List<SegmentDTO>();
    }
}
