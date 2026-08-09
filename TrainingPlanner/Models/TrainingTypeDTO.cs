using System;

namespace TrainingPlanner.Models
{
    public class TrainingTypeDTO(int Id, string Name, string Description="", bool IsSelected=true)
    {
        public int Id { get; set; } = Id;
        public string Name { get; set; } = Name;
        public string Description { get; set; } = Description;
        public bool IsSelected { get; set; } = IsSelected;
    }
}
