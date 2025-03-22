using TasksManagementSystem.EF.Entities;

namespace TasksManagementSystem.DAL.Interfaces
{
    public interface IProjectRepository
    {
        Task<ProjectEntity> InsertProject(ProjectEntity entity);

        ProjectEntity UpdateProject(ProjectEntity entity);

        void DeleteProject(ProjectEntity entity);

        Task<IEnumerable<ProjectEntity>> GetAllProjectsPaging(int skip, int take, string sortOrder);

        Task<IEnumerable<ProjectEntity>> GetAllTasksAssignedToAnEmployeeGroupedByProjects(string employeeId);

        Task<IEnumerable<ProjectEntity>> GetAllProjects();

        Task<ProjectEntity?> GetProjectById(int id);

        Task<int> CountProjects();

        Task<int> CommitAsync();

    }
}
