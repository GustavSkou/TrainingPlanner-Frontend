namespace TrainingPlanner.Models
{
    public class UserDTO
    {
        public int? Id { get; set; } = null;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string LoginProvider { get; set; } = string.Empty;   // fx github
        public string NameIdentifier { get; set; } = string.Empty;  // identifieren fra loginprovider        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public override string ToString()
        {
            return $"{UserName}\n{Email}\n{LoginProvider}\n{NameIdentifier}";
        }
    }
}