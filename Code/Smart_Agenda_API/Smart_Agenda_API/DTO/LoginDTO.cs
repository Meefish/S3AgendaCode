using System.ComponentModel.DataAnnotations;

namespace Smart_Agenda_API.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }

    }
}
