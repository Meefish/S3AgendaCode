using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Smart_Agenda_Logic.Domain
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("username")]
        public required string Username { get; set; }

        [Column("hashed_password")]
        public required string PasswordHash { get; set; }
        [Column("email")]
        public required string Email { get; set; }
        [Column("user_role")]
        public UserRole UserRole { get; set; }
        [JsonIgnore]
        public Calendar? Calendar { get; set; }

    }

    public enum UserRole
    {
        User,
        Admin
    }
}
