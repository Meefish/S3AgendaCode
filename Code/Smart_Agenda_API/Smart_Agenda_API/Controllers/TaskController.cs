using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_Agenda_API.DTO;
using Smart_Agenda_API.Mapper;
using Smart_Agenda_Logic.Exceptions;
using Smart_Agenda_Logic.Managers;

namespace Smart_Agenda_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private readonly TaskManager _taskManager;

        public TaskController(TaskManager taskManager)
        {
            _taskManager = taskManager;
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> AddTask(TaskCreationDTO taskCreationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Smart_Agenda_Logic.Domain.Task taskEntity = TaskMapper.ToEntity(taskCreationDTO);
                Smart_Agenda_Logic.Domain.Task createdTask = await _taskManager.AddTask(taskEntity);
                return Ok(createdTask);
            }
            catch (TaskException taskEx)
            {
                return StatusCode(500, $"Error adding task: {taskEx.Message}");
            }
        }

        /*
        [HttpGet("{id}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetTask(int id)
        {
            try
            {
                Smart_Agenda_Logic.Domain.Task task = await _taskManager.GetTask(id);
                return Ok(task);
            }
            catch (TaskException taskEx)
            {
                return StatusCode(500, $"Error getting task: {taskEx.Message}");
            }
        }
        */

        [HttpPut("{id}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> UpdateTask(int id, TaskUpdateDTO taskUpdateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Smart_Agenda_Logic.Domain.Task existingTask = await _taskManager.GetTask(id);
                Smart_Agenda_Logic.Domain.Task updatedTask = TaskMapper.UpdateEntity(existingTask, taskUpdateDTO);
                Smart_Agenda_Logic.Domain.Task result = await _taskManager.UpdateTask(updatedTask);
                return Ok(updatedTask);
            }
            catch (TaskException taskEx)
            {
                return StatusCode(500, $"Error updating task: {taskEx.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                Smart_Agenda_Logic.Domain.Task deletedTask = await _taskManager.DeleteTask(id);
                return Ok(deletedTask);
            }
            catch (TaskException taskEx)
            {
                return StatusCode(500, $"Error deleting task: {taskEx.Message}");
            }
        }


    }
}
