using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Utils.DTOs;
using TaskManagementSystem.Utils.ExtentionMethods;
using TasksManagementSystem.DAL.Interfaces;
using TasksManagementSystem.EF.Entities;
using TaskStatus = TasksManagementSystem.EF.Entities.TaskStatus;

namespace TasksManagementSystem.BL.Services
{
    public class TaskServices
    {
        private ITaskRepository _task;
        private IProjectRepository _project;
        private readonly UserManager<ApplicationUserEntity> _userManager;
       
        public TaskServices(ITaskRepository task, IProjectRepository project, UserManager<ApplicationUserEntity> userManager)
        {
            this._task = task;
            this._project = project;
            this._userManager = userManager;
        }
        
        public async Task<TaskDTO> CreateTask(TaskInputDTO taskInputDTO)
        {
            if (taskInputDTO is null)
                throw new ArgumentNullException("TaskInputDTO", "TaskInputDTO is required");

            if (string.IsNullOrEmpty(taskInputDTO.CreatedBy))
                throw new ArgumentException("The creator id must not be null", "CreatedBy");

            if (string.IsNullOrEmpty(taskInputDTO.AssignedTo))
                throw new ArgumentException("The assigned id must not be null", "AssignedTo");

            if (taskInputDTO.ProjectId <= 0)
                throw new ArgumentException("The project id must be greater than zero", "ProjectId");

            var userCreator = await this._userManager.FindByIdAsync(taskInputDTO.CreatedBy);

            if (userCreator is null)
                throw new ArgumentNullException("CreatedBy", "There is no user with this id");

            var userAssigned = await this._userManager.FindByIdAsync(taskInputDTO.AssignedTo);

            if (userAssigned is null)
                throw new ArgumentNullException("AssignedTo", "There is no user with this id");

            var project = await this._project.GetProjectById(taskInputDTO.ProjectId);

            if (project is null)
                throw new ArgumentNullException("ProjectId", "There is no project with this id");

            var taskToInsert = taskInputDTO.ToTaskEntity();
            taskToInsert.Status = TaskStatus.PENDING;

            var result = await this._task.InsertTask(taskToInsert);

            await this._task.CommitAsync();

            return result.ToTaskDTO();
        }
      
        public async Task<bool> DeleteTask(int taskId)
        {
            if (taskId <= 0)
                throw new ArgumentException("The task id must be greater than zero", "TaskId");


            var task = await this._task.GetTaskById(taskId);

            if (task is null)
                throw new ArgumentNullException("TaskId", "There is no task with this id");

            this._task.DeleteTask(task);

            var response = await this._task.CommitAsync();

            return response > 0;
        }
      
        public async Task<TaskDTO> EditTask(TaskInputDTO taskInputDTO)
        {
            if (taskInputDTO is null)
                throw new ArgumentNullException("TaskInputDTO", "TaskInputDTO is required");

            if (taskInputDTO.Id <= 0)
                throw new ArgumentException("The task id must be greater than zero", "TaskId");

            if (string.IsNullOrEmpty(taskInputDTO.CreatedBy))
                throw new ArgumentException("The creater id must not be null", "CreatedBy");

            if (string.IsNullOrEmpty(taskInputDTO.AssignedTo))
                throw new ArgumentException("The user assigned id must not be null", "AssignedTo");

            var userAssigned = await this._userManager.FindByIdAsync(taskInputDTO.AssignedTo);

            if (userAssigned is null)
                throw new ArgumentNullException("AssignedTo", "There is no user with this id");

            var userCreator = await this._userManager.FindByIdAsync(taskInputDTO.CreatedBy);

            if (userCreator is null)
                throw new ArgumentNullException("CreatedBy", "There is no user with this id");

            var taskForUpdate = await this._task.GetTaskById(taskInputDTO.Id);

            if (taskForUpdate is null)
                throw new ArgumentNullException("TaskId", "There is no task with this id");

            taskForUpdate.Title = taskInputDTO.Title;
            taskForUpdate.Description = taskInputDTO.Description;
            taskForUpdate.Status = (TaskStatus)taskInputDTO.Status;
            taskForUpdate.AssignedToId = taskInputDTO.AssignedTo;

            var result = this._task.UpdateTask(taskForUpdate);

            await this._task.CommitAsync();

            return result.ToTaskDTO();
        }

        public async Task<TaskDTO> AppoveTask(int taskId)
        {

            if (taskId <= 0)
                throw new ArgumentException("The task id must be greater than zero", "TaskId");


            var taskForUpdate = await this._task.GetTaskById(taskId);

            if (taskForUpdate is null)
                throw new ArgumentNullException("TaskId", "There is no task with this id");

            taskForUpdate.Status = TaskStatus.APPROVED;

            var result = this._task.UpdateTask(taskForUpdate);

            await this._task.CommitAsync();

            return result.ToTaskDTO();
        }
        
        public async Task<TaskDTO> RejectTask(int taskId)
        {

            if (taskId <= 0)
                throw new ArgumentException("The task id must be greater than zero", "TaskId");


            var taskForUpdate = await this._task.GetTaskById(taskId);

            if (taskForUpdate is null)
                throw new ArgumentNullException("TaskId", "There is no task with this id");

            taskForUpdate.Status = TaskStatus.NOT_APPROVED;
            taskForUpdate.CanStatusSetApprovedByUser = true;
            var result = this._task.UpdateTask(taskForUpdate);

            await this._task.CommitAsync();

            return result.ToTaskDTO();
        }
       
        public async Task<TaskDTO> AppoveTaskByEmployee(string employeeId, int taskId)
        {
            if (taskId <= 0)
                throw new ArgumentException("The task id must be greater than zero", "TaskId");

            if (string.IsNullOrEmpty(employeeId))
                throw new ArgumentException("The employee id must not be null", "CreatedBy");

            var employee = await this._userManager.FindByIdAsync(employeeId);

            if (employee is null)
                throw new ArgumentNullException("EmployeeId", "There is no user with this id");

            var taskForUpdate = await this._task.GetTaskById(taskId);

            if (taskForUpdate is null)
                throw new ArgumentNullException("TaskId", "There is no task with this id");

            if (taskForUpdate.AssignedToId != employeeId)
                throw new ArgumentNullException("AssignedToId", "This task not assigned to the editor employee");

            taskForUpdate.Status = TaskStatus.APPROVED;

            var result = this._task.UpdateTask(taskForUpdate);

            await this._task.CommitAsync();

            return result.ToTaskDTO();
        }
       
        public async Task<TasksInfoDTO> GetTasksByProjectId(int projectId, PaggingParamDTO paggingParam)
        {
            if (paggingParam is null)
                throw new ArgumentNullException("PaggingParamDTO", "PaggingParamDTO is required");

            if (projectId <= 0)
                throw new ArgumentException("The project id must be greater than zero", "ProjectId");


            var project = await this._project.GetProjectById(projectId);

            if (project is null)
                throw new ArgumentNullException("ProjectId", "There is no project with this id");


            TasksInfoDTO response = new TasksInfoDTO();
            response.Tasks = new List<TaskDTO>();
            string sortOrder = "CreatedAt DESC";

            if (!string.IsNullOrEmpty(sortOrder))
                sortOrder = $"{paggingParam.OrderBy} {paggingParam.SortOrder.ToUpper()}";

            if (paggingParam.Skip < 0)
                paggingParam.Skip = 0;

            if (paggingParam.Take < 5)
                paggingParam.Take = 5;

            var result = await this._task.GetAllTasksPagingByProjectId(projectId, paggingParam.Skip, paggingParam.Take, sortOrder);

            // Fetch all user data in one query to avoid N+1 issue
            var userIds = result.SelectMany(t => new[] { t.CreatedById, t.AssignedToId }).Distinct().ToList();
            var usersDict = (await _userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync())
                .ToDictionary(u => u.Id);

            foreach (var task in result)
            {
                TaskDTO taskDTO = task.ToTaskDTO();

                if (usersDict.TryGetValue(task.CreatedById, out var userCreator))
                {
                    taskDTO.CreatedByEmail = userCreator.Email;
                    taskDTO.CreatedByName = $"{userCreator.FirstName} {userCreator.LastName}";
                }

                if (usersDict.TryGetValue(task.AssignedToId, out var userAssigned))
                {
                    taskDTO.AssignedToEmail = userAssigned.Email;
                    taskDTO.AssignedToName = $"{userAssigned.FirstName} {userAssigned.LastName}";
                }

                taskDTO.ProjectId = (int)task.ProjectId;
                response.Tasks.Add(taskDTO);
            }

            response.TasksCount = await this._task.CountTasks();
            response.ProjectId = projectId;
            response.ProjectName = project.Name;
            return response;
        }

        public async Task<TasksInfoDTO> GetTasksForAnEmployeeByProjectId(string employeeId, int projectId, PaggingParamDTO paggingParam)
        {
            if (paggingParam is null)
                throw new ArgumentNullException("PaggingParamDTO", "PaggingParamDTO is required");

            if (projectId <= 0)
                throw new ArgumentException("The project id must be greater than zero", "ProjectId");

            if (string.IsNullOrEmpty(employeeId))
                throw new ArgumentException("The employee id must not be null", "EmployeeId");

            var employee = await this._userManager.FindByIdAsync(employeeId);

            if (employee is null)
                throw new ArgumentNullException("EmployeeId", "There is no user with this id");

            var project = await this._project.GetProjectById(projectId);

            if (project is null)
                throw new ArgumentNullException("ProjectId", "There is no project with this id");


            TasksInfoDTO response = new TasksInfoDTO();
            response.Tasks = new List<TaskDTO>();
            string sortOrder = "CreatedAt DESC";

            if (!string.IsNullOrEmpty(sortOrder))
                sortOrder = $"{paggingParam.OrderBy} {paggingParam.SortOrder.ToUpper()}";

            if (paggingParam.Skip < 0)
                paggingParam.Skip = 0;

            if (paggingParam.Take < 5)
                paggingParam.Take = 5;

            var result = await this._task.GetAllTasksForAnEmployeePagingByProjectId(employeeId, projectId, paggingParam.Skip, paggingParam.Take, sortOrder);

            // Fetch all user data in one query to avoid N+1 issue
            var userIds = result.SelectMany(t => new[] { t.CreatedById, t.AssignedToId }).Distinct().ToList();
            var usersDict = (await _userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync())
                .ToDictionary(u => u.Id);

            foreach (var task in result)
            {
                TaskDTO taskDTO = task.ToTaskDTO();

                if (usersDict.TryGetValue(task.CreatedById, out var userCreator))
                {
                    taskDTO.CreatedByEmail = userCreator.Email;
                    taskDTO.CreatedByName = $"{userCreator.FirstName} {userCreator.LastName}";
                }

                if (usersDict.TryGetValue(task.AssignedToId, out var userAssigned))
                {
                    taskDTO.AssignedToEmail = userAssigned.Email;
                    taskDTO.AssignedToName = $"{userAssigned.FirstName} {userAssigned.LastName}";
                }

                taskDTO.ProjectId = (int)task.ProjectId;
                response.Tasks.Add(taskDTO);
            }

            response.TasksCount = await this._task.CountTasksForAnEmployee(employeeId);
            response.ProjectId = projectId;
            response.ProjectName = project.Name;
            return response;
        }

        public async Task<TaskDTO> GetTaskById(int taskId)
        {
            if (taskId <= 0)
                throw new ArgumentException("The task id must be greater than zero", "TaskId");


            var result = await this._task.GetTaskById(taskId);

            if (result is null)
                throw new ArgumentNullException("TaskId", "There is no task with this id");

            TaskDTO response = new TaskDTO();

            response = result.ToTaskDTO();

            ApplicationUserEntity userCreator = await this._userManager.FindByIdAsync(result.CreatedById);
            ApplicationUserEntity userAssigned = await this._userManager.FindByIdAsync(result.AssignedToId);

            response.CreatedByEmail = userCreator.Email;

            response.CreatedByName = $"{userCreator.FirstName} {userCreator.LastName}";

            response.AssignedToEmail = userAssigned.Email;

            response.AssignedToName = $"{userAssigned.FirstName} {userAssigned.LastName}";

            response.ProjectId = (int)result.ProjectId;

            return response;
        }

        public async Task<TaskDTO> GetEmployeeTaskById(string employeeId, int taskId)
        {
            if (taskId <= 0)
                throw new ArgumentException("The task id must be greater than zero", "TaskId");

            if (string.IsNullOrEmpty(employeeId))
                throw new ArgumentException("The employee id must not be null", "CreatedBy");

            var employee = await this._userManager.FindByIdAsync(employeeId);

            if (employee is null)
                throw new ArgumentNullException("EmployeeId", "There is no user with this id");


            var result = await this._task.GetEmployeeTaskById(employeeId, taskId);

            if (result is null)
                throw new ArgumentNullException("TaskId", "There is no task with this id");

            TaskDTO response = new TaskDTO();

            response = result.ToTaskDTO();

            ApplicationUserEntity userCreator = await this._userManager.FindByIdAsync(result.CreatedById);
            ApplicationUserEntity userAssigned = await this._userManager.FindByIdAsync(result.AssignedToId);

            response.CreatedByEmail = userCreator.Email;

            response.CreatedByName = $"{userCreator.FirstName} {userCreator.LastName}";

            response.AssignedToEmail = userAssigned.Email;

            response.AssignedToName = $"{userAssigned.FirstName} {userAssigned.LastName}";

            response.ProjectId = (int)result.ProjectId;

            return response;
        }

        public async Task<TaskDTO> MakeTaskCompletedByEmployee(string employeeId, int taskId)
        {
            if (taskId <= 0)
                throw new ArgumentException("The task id must be greater than zero", "TaskId");

            if (string.IsNullOrEmpty(employeeId))
                throw new ArgumentException("The employee id must not be null", "CreatedBy");

            var employee = await this._userManager.FindByIdAsync(employeeId);

            if (employee is null)
                throw new ArgumentNullException("EmployeeId", "There is no user with this id");

            var taskForUpdate = await this._task.GetTaskById(taskId);

            if (taskForUpdate is null)
                throw new ArgumentNullException("TaskId", "There is no task with this id");

            if (taskForUpdate.AssignedToId != employeeId)
                throw new ArgumentNullException("AssignedToId", "This task not assigned to the editor employee");

            taskForUpdate.Status = TaskStatus.COMPLETED;

            var result = this._task.UpdateTask(taskForUpdate);

            await this._task.CommitAsync();

            return result.ToTaskDTO();
        }

    }
}
