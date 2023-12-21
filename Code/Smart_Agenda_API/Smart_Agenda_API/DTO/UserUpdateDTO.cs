using System.ComponentModel.DataAnnotations;

namespace Smart_Agenda_API.DTO
{
    public class UserUpdateDTO
    {
        public string? Name { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }
        public string? Password { get; set; }

    }
}
