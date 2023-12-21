using Smart_Agenda_Logic.Domain;

namespace Smart_Agenda_API.DTO
{
    public class TaskUpdateDTO
    {
        public string? TaskName { get; set; }
        public DateTime DueDate { get; set; }

        public TaskPriority TaskPriority { get; set; }

        public bool Status { get; set; }
    }
}
