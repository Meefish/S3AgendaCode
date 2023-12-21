using Smart_Agenda_API.DTO;

namespace Smart_Agenda_API.Mapper
{
    public static class TaskMapper
    {

        public static Smart_Agenda_Logic.Domain.Task ToEntity(TaskCreationDTO creationDTO)
        {
            if (creationDTO == null)
            {
                return null;
            }

            return new Smart_Agenda_Logic.Domain.Task
            {
                TaskName = creationDTO.TaskName,
                DueDate = creationDTO.DueDate,
                TaskPriority = creationDTO.TaskPriority,
                Status = creationDTO.Status,
                CalendarId = creationDTO.CalendarId
            };
        }

        public static Smart_Agenda_Logic.Domain.Task UpdateEntity(Smart_Agenda_Logic.Domain.Task existingTask, TaskUpdateDTO updatedTask)
        {
            if (updatedTask == null)
            {
                return existingTask;
            }

            if (updatedTask.TaskName != null)
            {
                existingTask.TaskName = updatedTask.TaskName;
            }

            existingTask.DueDate = updatedTask.DueDate;

            existingTask.TaskPriority = updatedTask.TaskPriority;

            existingTask.Status = updatedTask.Status;


            return existingTask;
        }
    }
}
