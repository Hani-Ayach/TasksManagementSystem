using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Utils.DTOs;
using TaskManagementSystem.Utils.ExtentionMethods;
using TasksManagementSystem.DAL.Interfaces;
using TasksManagementSystem.EF.Entities;

namespace TasksManagementSystem.BL.Services
{
    public class ProjectServices
    {
        private IProjectRepository _project;
        private ITaskRepository _task;
        private readonly UserManager<ApplicationUserEntity> _userManager;
      
        public ProjectServices(IProjectRepository project, ITaskRepository task, UserManager<ApplicationUserEntity> userManager)
        {
            this._project = project;
            this._task = task;
            this._userManager = userManager;
        }
       
        public async Task<ProjectDTO> CreateProject(ProjectInputDTO projectInputDTO)
        {
            if (projectInputDTO is null)
                throw new ArgumentNullException("ProjectInputDTO", "ProjectInputDTO is required");

            if (string.IsNullOrEmpty(projectInputDTO.CreatedBy))
                throw new ArgumentException("The creater id must not be null", "CreatedBy");

            var userCreator = await this._userManager.FindByIdAsync(projectInputDTO.CreatedBy);

            if (userCreator is null)
                throw new ArgumentNullException("CreatedBy", "There is no user with this id");

            var result = await this._project.InsertProject(projectInputDTO.ToProjectEntity());

            await this._project.CommitAsync();

            return result.ToProjectDTO();
        }
       
        public async Task<bool> DeleteProject(int projectId)
        {
            if (projectId <= 0)
                throw new ArgumentException("The project id must be greater than zero", "ProjectId");


            var project = await this._project.GetProjectById(projectId);

            if (project is null)
                throw new ArgumentNullException("ProjectId", "There is no project with this id");

            this._project.DeleteProject(project);

            var response = await this._project.CommitAsync();

            return response > 0;
        }
       
        public async Task<ProjectDTO> EditProject(ProjectInputDTO projectInputDTO)
        {
            if (projectInputDTO is null)
                throw new ArgumentNullException("ProjectInputDTO", "ProjectInputDTO is required");

            if (projectInputDTO.Id <= 0)
                throw new ArgumentException("The project id must be greater than zero", "ProjectId");

            if (string.IsNullOrEmpty(projectInputDTO.CreatedBy))
                throw new ArgumentException("The creater id must not be null", "CreatedBy");

            var userCreator = await this._userManager.FindByIdAsync(projectInputDTO.CreatedBy);

            if (userCreator is null)
                throw new ArgumentNullException("CreatedBy", "There is no user with this id");

            var projectForUpdate = await this._project.GetProjectById(projectInputDTO.Id);

            if (projectForUpdate is null)
                throw new ArgumentNullException("ProjectId", "There is no project with this id");

            projectForUpdate.Name = projectInputDTO.Name;
            projectForUpdate.Description = projectInputDTO.Description;

            var result = this._project.UpdateProject(projectForUpdate);

            var r = await this._project.CommitAsync();

            return result.ToProjectDTO();
        }

        public async Task<ProjectsInfoDTO> GetProjects(PaggingParamDTO paggingParam)
        {
            if (paggingParam is null)
                throw new ArgumentNullException("PaggingParamDTO", "PaggingParamDTO is required");


            ProjectsInfoDTO response = new ProjectsInfoDTO();
            response.Projects = new List<ProjectDTO>();

            string sortOrder = "CreatedAt DESC";

            if (!string.IsNullOrEmpty(sortOrder))
                sortOrder = $"{paggingParam.OrderBy} {paggingParam.SortOrder.ToUpper()}";

            if (paggingParam.Skip < 0)
                paggingParam.Skip = 0;

            if (paggingParam.Take < 5)
                paggingParam.Take = 5;

            var result = await this._project.GetAllProjectsPaging(paggingParam.Skip, paggingParam.Take, sortOrder);

            // Fetch all user data in one query to avoid N+1 issue
            var userIds = result.Select(p => p.CreatedById).Distinct().ToList();
            var usersDict = (await _userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync())
                .ToDictionary(u => u.Id);


            foreach (var project in result)
            {
                ProjectDTO projectDTO = project.ToProjectDTO();

                projectDTO.TasksCount = await this._task.CountTasksPerProject(project.ProjectId);

                if (usersDict.TryGetValue(project.CreatedById, out var userCreator))
                {
                    projectDTO.CreatedByEmail = userCreator.Email;
                    projectDTO.CreatedByName = $"{userCreator.FirstName} {userCreator.LastName}";
                }

                response.Projects.Add(projectDTO);
            }
            response.ProjectsCount = await this._project.CountProjects();

            return response;
        }

        public async Task<List<ProjectDTO>> GroupAssignedTasksByProjects(string employeeId)
        {
            List<ProjectDTO> response = new List<ProjectDTO>();

            if (string.IsNullOrEmpty(employeeId))
                throw new ArgumentException("The employee id must not be null", "EmployeeId");

            var employee = await this._userManager.FindByIdAsync(employeeId);

            if (employee is null)
                throw new ArgumentNullException("EmployeeId", "There is no user with this id");


            var result = await this._project.GetAllTasksAssignedToAnEmployeeGroupedByProjects(employeeId);

            // Fetch all user data in one query to avoid N+1 issue
            var userIds = result.Select(p => p.CreatedById).Distinct().ToList();
            var usersDict = (await _userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync())
                .ToDictionary(u => u.Id);


            foreach (var project in result)
            {
                if (project.TaskEntities.Count() > 0)
                {
                    ProjectDTO projectDTO = project.ToProjectDTO();

                    projectDTO.TasksCount = project.TaskEntities.Count();

                    if (usersDict.TryGetValue(project.CreatedById, out var userCreator))
                    {
                        projectDTO.CreatedByEmail = userCreator.Email;
                        projectDTO.CreatedByName = $"{userCreator.FirstName} {userCreator.LastName}";
                    }
                    response.Add(projectDTO);
                }
            }

            return response;
        }
        
        public async Task<ProjectDTO> GetProjectById(int projectId)
        {
            if (projectId <= 0)
                throw new ArgumentException("The project id must be greater than zero", "ProjectId");


            var result = await this._project.GetProjectById(projectId);

            if (result is null)
                throw new ArgumentNullException("ProjectId", "There is no project with this id");

            ProjectDTO response = new ProjectDTO();

            response = result.ToProjectDTO();

            ApplicationUserEntity userCreator = await this._userManager.FindByIdAsync(result.CreatedById);

            response.CreatedByEmail = userCreator.Email;

            response.CreatedByName = $"{userCreator.FirstName} {userCreator.LastName}";

            response.TasksCount = await this._task.CountTasksPerProject(projectId);

            return response;
        }

    }
}
