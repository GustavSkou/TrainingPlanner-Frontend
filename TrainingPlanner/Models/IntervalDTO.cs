namespace TrainingPlanner.Models
{
    public class IntervalDTO
    {
        public int? Id { get; set; }
        public int SegmentId { get; set; }
        public int Order { get; set; }
        public int? DistanceMeters { get; set; }
        public int? DurationSeconds { get; set; }
        public int? TargetPaceSecondsPerKm { get; set; }
        public int? TargetPaceSecondsPerKmUpperBound { get; set; }
        public int? TargetPaceSecondsPerKmLowerBound { get; set; }
        public string? Notes { get; set; }
    }
}