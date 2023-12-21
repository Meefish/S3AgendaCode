using System.ComponentModel.DataAnnotations;

namespace Smart_Agenda_API.DTO
{
    public class UserCreationDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }
    }
}
