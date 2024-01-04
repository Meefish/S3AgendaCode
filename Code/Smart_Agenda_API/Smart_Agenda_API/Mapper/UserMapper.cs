using Smart_Agenda_API.DTO;
using Smart_Agenda_Logic.Domain;

namespace Smart_Agenda_API.Mapper
{
    public static class UserMapper
    {

        public static User ToEntity(UserCreationDTO creationDTO)
        {
            if (creationDTO == null)
            {
                return null;
            }

            return new User
            {
                Username = creationDTO.Name,
                Email = creationDTO.Email,
                PasswordHash = PasswordHasher.HashPassword(creationDTO.Password)
            };
        }

        public static User UpdateEntity(User existingUser, UserUpdateDTO updatedUser)
        {
            if (updatedUser == null)
            {
                return existingUser;
            }

            if (!string.IsNullOrEmpty(updatedUser.Name))
            {
                existingUser.Username = updatedUser.Name;
            }

            if (!string.IsNullOrEmpty(updatedUser.Email))
            {
                existingUser.Email = updatedUser.Email;
            }

            if (!string.IsNullOrEmpty(updatedUser.Password))
            {
                existingUser.PasswordHash = PasswordHasher.HashPassword(updatedUser.Password);
            }

            return existingUser;
        }

    }
}
