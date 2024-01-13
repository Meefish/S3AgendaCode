using Smart_Agenda_Logic.Domain;
using Smart_Agenda_Logic.Exceptions;
using Smart_Agenda_Logic.Interfaces;

namespace Smart_Agenda_Logic.Managers
{
    public class UserManager
    {
        private readonly IUserDAL _userDAL;
        public UserManager(IUserDAL userDAL)
        {
            _userDAL = userDAL;
        }

        private async Task<T> ExecuteUserOperationAsync<T>(Func<Task<T>> operation, Func<Exception, UserException> exceptionTransformer)
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
        public async Task<User> AddUser(User user)
        {
            return await ExecuteUserOperationAsync(async () =>
            {
                if (string.IsNullOrEmpty(user.Username))
                {
                    throw new AddUserException("Username can't be empty.");
                }

                if (user.Username.Length > 12)
                {
                    throw new AddUserException("Username can't exceed 12 characters.");
                }

                if (string.IsNullOrEmpty(user.Email))
                {
                    throw new AddUserException("Email can't be empty.");
                }

                return await _userDAL.AddUser(user);
            }, ex => new UserException(ex.Message, ex));
        }
        public async Task<User> GetUser(int id)
        {
            return await ExecuteUserOperationAsync(async () =>
            {
                if (id <= 0)
                {
                    throw new RetrieveUserException("The user doesn't exist.");
                }
                return await _userDAL.GetUser(id);
            }, ex => new UserException(ex.Message, ex));
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await ExecuteUserOperationAsync(async () =>
            {
                return await _userDAL.GetAllUsers();
            }, ex => new UserException(ex.Message, ex));
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await ExecuteUserOperationAsync(async () =>
            {
                if (string.IsNullOrEmpty(email))
                {
                    throw new RetrieveUserException("The user doesn't exist.");
                }
                return await _userDAL.GetUserByEmail(email);
            }, ex => new UserException(ex.Message, ex));
        }

        public async Task<User> UpdateUser(User user)
        {
            return await ExecuteUserOperationAsync(async () =>
            {
                if (string.IsNullOrEmpty(user.Username))
                {
                    throw new UpdateUserException("Username can't be empty.");
                }

                if (string.IsNullOrEmpty(user.Email))
                {
                    throw new UpdateUserException("Email can't be empty.");
                }

                if (user.Username.Length > 12)
                {
                    throw new UpdateUserException("Username can't exceed 12 characters.");
                }

                if (user.UserId <= 0)
                {
                    throw new UpdateUserException("The user doesn't exist.");
                }

                return await _userDAL.UpdateUser(user);
            }, ex => new UserException(ex.Message, ex));
        }
        public async Task<User> DeleteUser(int id)
        {
            return await ExecuteUserOperationAsync(async () =>
            {
                if (id <= 0)
                {
                    throw new DeleteUserException("The user doesn't exist.");
                }
                return await _userDAL.DeleteUser(id);
            }, ex => new UserException(ex.Message, ex));
        }


    }
}
