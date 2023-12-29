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

        [HttpGet("get")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> RetrieveAllCalendarTasks(int calendarId)
        {
            try
            {
                List<Smart_Agenda_Logic.Domain.Task> tasks = await _calendarManager.GetAllCalendarTasks(calendarId);
                return Ok(tasks);
            }
            catch (CalendarException calendarEx)
            {
                return StatusCode(500, $"Error retrieving calendar tasks: {calendarEx.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAllCalendarTasks(int calendarId)
        {
            try
            {
                await _calendarManager.DeleteAllCalendarTasks(calendarId);
                return Ok();
            }
            catch (CalendarException calendarEx)
            {
                return StatusCode(500, $"Error deleting calendar tasks: {calendarEx.Message}");
            }
        }
    }
}
