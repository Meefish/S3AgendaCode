using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_Agenda_API.DTO;
using Smart_Agenda_API.Mapper;
using Smart_Agenda_Logic.Domain;
using Smart_Agenda_Logic.Exceptions;
using Smart_Agenda_Logic.Managers;

namespace Smart_Agenda_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserManager _userManager;
        private readonly CalendarManager _calendarManager;
        private readonly IJwtService _jwtService;
        public UserController(UserManager userManager, CalendarManager calendarManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _calendarManager = calendarManager;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUser(UserCreationDTO userCreationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                User userEntity = UserMapper.ToEntity(userCreationDTO);
                User createdUser = await _userManager.AddUser(userEntity);
                return Ok(createdUser);
            }
            catch (UserException userEx)
            {
                return StatusCode(500, $"Error adding user: {userEx.Message}");
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                User user = await _userManager.GetUserByEmail(loginDTO.Email);

                var passwordValid = PasswordHasher.VerifyPassword(loginDTO.Password, user.PasswordHash);
                if (!passwordValid)
                {
                    return Unauthorized("Invalid password");
                }

                var calendar = await _calendarManager.GetCalendarForUser(user.UserId);
                var token = _jwtService.GenerateJwtToken(user, calendar.CalendarId);
                return Ok(new { Token = token });
            }
            catch (UserException userEx)
            {
                return NotFound($"Error getting user: {userEx.Message}");
            }
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                User user = await _userManager.GetUser(id);
                return Ok(user);
            }
            catch (UserException userEx)
            {
                return NotFound($"Error getting user: {userEx.Message}");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                List<User> users = await _userManager.GetAllUsers();
                return Ok(users);
            }
            catch (UserException userEx)
            {
                return NotFound($"Error getting users: {userEx.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                User existingUser = await _userManager.GetUser(id);
                User updatedUser = UserMapper.UpdateEntity(existingUser, user);
                User result = await _userManager.UpdateUser(updatedUser);
                return Ok(result);
            }
            catch (UserException userEx)
            {
                return NotFound($"Error getting user: {userEx.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                User deletedUser = await _userManager.DeleteUser(id);
                return Ok(deletedUser);
            }
            catch (UserException userEx)
            {
                return NotFound($"Error deleting user: {userEx.Message}");
            }
        }






    }
}
