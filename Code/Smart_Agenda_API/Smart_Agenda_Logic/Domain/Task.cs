using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Agenda_Logic.Domain
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("task_id")]
        public int TaskId { get; set; }
        [Column("task_name")]
        public required string TaskName { get; set; }
        [Column("due_date")]
        public DateTime DueDate { get; set; }
        [Column("task_priority")]
        public TaskPriority TaskPriority { get; set; }
        [Column("status")]
        public bool Status { get; set; }

        [Column("calendar_id")]
        public int CalendarId { get; set; }
        public Calendar? Calendar { get; set; }
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High,
        Urgent
    }
}
