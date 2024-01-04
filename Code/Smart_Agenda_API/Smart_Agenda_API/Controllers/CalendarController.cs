using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_Agenda_Logic.Exceptions;
using Smart_Agenda_Logic.Managers;

namespace Smart_Agenda_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalendarController : Controller
    {

        private readonly CalendarManager _calendarManager;
        public CalendarController(CalendarManager calendarManager)
        {
            _calendarManager = calendarManager;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetAllCalendarTasks(int id)
        {
            try
            {
                List<Smart_Agenda_Logic.Domain.Task> tasks = await _calendarManager.GetAllCalendarTasks(id);
                return Ok(tasks);
            }
            catch (CalendarException calendarEx)
            {
                return NotFound($"Error retrieving calendar tasks: {calendarEx.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAllCalendarTasks(int id)
        {
            try
            {
                await _calendarManager.DeleteAllCalendarTasks(id);
                return Ok();
            }
            catch (CalendarException calendarEx)
            {
                return NotFound($"Error deleting calendar tasks: {calendarEx.Message}");
            }
        }
    }
}
