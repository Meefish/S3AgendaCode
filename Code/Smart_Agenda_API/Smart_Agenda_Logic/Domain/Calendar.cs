using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Agenda_Logic.Domain
{
    public class Calendar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("calendar_id")]
        public int CalendarId { get; set; }
        [Column("preference")]
        public string? Preference { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }
        public User? User { get; set; }

        public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    }
}
