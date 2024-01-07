using Smart_Agenda_Logic.Domain;
using Smart_Agenda_Logic.Exceptions;
using Smart_Agenda_Logic.Interfaces;

namespace Logic.UnitTest
{
    public class MockUserDAL : IUserDAL
    {
        private readonly List<User> _database = new List<User>();
        private int _idCounter = 1;

        public MockUserDAL()
        {
            var mockUser = new User
            {
                UserId = _idCounter++,
                Username = "Jan89",
                PasswordHash = "$11$P / JQHRlgEei3UB3DKBeg3OXXVy1lWr1 / KS8ISQzhNpldi6Cfc9qQ2",
                Email = "jan89@example.com",
                UserRole = UserRole.User,
            };
            _database.Add(mockUser);
        }

        public Task<User> AddUser(User user)
        {

            user.UserId = _idCounter++;
            _database.Add(user);


            return System.Threading.Tasks.Task.FromResult(user);
        }

        public Task<User> GetUser(int id)
        {
            var user = _database.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                throw new RetrieveUserException("User not found");
            }
            return System.Threading.Tasks.Task.FromResult(user);
        }
        public Task<List<User>> GetAllUsers()
        {
            throw new NotImplementedException();
        }
        public Task<User> GetUserByEmail(string email)
        {
            var user = _database.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                throw new RetrieveUserException("User not found");
            }
            return System.Threading.Tasks.Task.FromResult(user);
        }

        public Task<User> UpdateUser(User user)
        {

            var existingUser = _database.FirstOrDefault(u => u.UserId == user.UserId);
            if (existingUser == null)
            {
                throw new UpdateUserException("User not found");
            }

            existingUser.Username = user.Username;
            existingUser.Email = user.Email;


            return System.Threading.Tasks.Task.FromResult(existingUser);
        }

        public Task<User> DeleteUser(int id)
        {

            var user = _database.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                throw new DeleteUserException("User not found");
            }

            _database.Remove(user);


            return System.Threading.Tasks.Task.FromResult(user);
        }

    }
}
