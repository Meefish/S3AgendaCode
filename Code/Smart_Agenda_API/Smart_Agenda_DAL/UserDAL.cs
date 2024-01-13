using Microsoft.EntityFrameworkCore;
using Smart_Agenda_Logic.Domain;
using Smart_Agenda_Logic.Exceptions;
using Smart_Agenda_Logic.Interfaces;

namespace Smart_Agenda_DAL
{
    public class UserDAL : IUserDAL
    {
        private readonly DataBase _context;
        public UserDAL(DataBase context)
        {
            _context = context;
        }

        private async Task<T> ExecuteDbOperationAsync<T>(Func<Task<T>> operation, Func<Exception, Exception> exceptionTransformer)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw exceptionTransformer(ex);
            }
        }

        private async System.Threading.Tasks.Task CreateCalendarForUser(User user)
        {
            var calendar = new Calendar { UserId = user.UserId };
            _context.Calendar.Add(calendar);
            await _context.SaveChangesAsync();
        }

        public async Task<User> AddUser(User user)
        {
            return await ExecuteDbOperationAsync(async () =>
            {
                _context.User.Add(user);

                await _context.SaveChangesAsync();
                await CreateCalendarForUser(user);
                return user;
            }, ex => new AddUserException("Adding a new user went wrong", ex));

        }

        public async Task<User> GetUser(int id)
        {
            return await ExecuteDbOperationAsync(async () =>
            {
                var user = await _context.User.FindAsync(id);
                if (user == null)
                {
                    throw new RetrieveUserException("User not found");
                }
                return user;
            }, ex => new RetrieveUserException("Retrieving a user went wrong", ex));
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await ExecuteDbOperationAsync(async () =>
            {
                var users = await _context.User.ToListAsync();
                if (users == null)
                {
                    throw new RetrieveUserException("Users not found");
                }
                return users;

            }, ex => new RetrieveUserException("Retrieving users went wrong", ex));
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await ExecuteDbOperationAsync(async () =>
            {
                var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    throw new RetrieveUserException("User not found");
                }
                return user;
            }, ex => new RetrieveUserException("Retrieving a user went wrong", ex));
        }

        public async Task<User> UpdateUser(User user)
        {
            return await ExecuteDbOperationAsync(async () =>
            {
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return user;
            }, ex => new UpdateUserException("Updating a user went wrong", ex));
        }

        public async Task<User> DeleteUser(int id)
        {
            return await ExecuteDbOperationAsync(async () =>
            {
                var user = await _context.User.FindAsync(id);
                if (user == null)
                {
                    throw new DeleteUserException("User not found");
                }

                _context.User.Remove(user);
                await _context.SaveChangesAsync();

                return user;
            }, ex => new DeleteUserException("Deleting a user went wrong", ex));
        }





    }
}
