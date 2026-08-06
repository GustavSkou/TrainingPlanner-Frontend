namespace TrainingPlanner.Models
{
    public class SegmentDTO
    {
        public int? Id { get; set; }
        public int WorkoutId { get; set; }
        public int Order { get; set; }
        public int RepeatCount { get; set; } = 1;
        public string? Notes { get; set; }
        public ICollection<IntervalDTO> Intervals { get; set; } = new List<IntervalDTO>();
    }
}