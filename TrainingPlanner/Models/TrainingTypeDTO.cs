using System.Text.Json.Serialization;

namespace TrainingPlanner.Models
{
    public class TrainingTypeDTO
    {
        public TrainingTypeDTO()
        {
        }

        public TrainingTypeDTO(int id, string name, string description = "", bool isSelected = true)
        {
            Id = id;
            Name = name;
            Description = description;
            IsSelected = isSelected;
        }

        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsSelected { get; set; } = true;
    }
}
