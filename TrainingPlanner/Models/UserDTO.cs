namespace TrainingPlanner.Models
{
    public class UserDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public override string ToString()
        {
            return $"{nameof(GetType)}\n{FirstName}\n{LastName}\n{Email}\n{CreatedAt}\n{UpdatedAt}";
        }
    }
}