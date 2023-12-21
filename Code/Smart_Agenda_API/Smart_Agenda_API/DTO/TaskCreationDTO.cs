using Smart_Agenda_Logic.Domain;
using System.ComponentModel.DataAnnotations;

namespace Smart_Agenda_API.DTO
{
    public class TaskCreationDTO
    {
        [Required(ErrorMessage = "Task name is required")]
        public required string TaskName { get; set; }

        [Required(ErrorMessage = "Due date is required")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Task priority is required")]
        public TaskPriority TaskPriority { get; set; }

        [Required(ErrorMessage = "Task status is required")]
        public bool Status { get; set; }

        [Required(ErrorMessage = "Calendar id is required")]
        public int CalendarId { get; set; }
    }
}
