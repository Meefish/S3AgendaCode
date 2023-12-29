using Smart_Agenda_Logic.Domain;

namespace Smart_Agenda_Logic.Interfaces
{
    public interface IUserDAL
    {

        Task<User> AddUser(User user);
        Task<User> GetUser(int id);
        Task<User> UpdateUser(User user);

        Task<User> DeleteUser(int id);

        //    bool DoesEmailExist(string email);

        Task<User> GetUserByEmail(string email);


    }
}
