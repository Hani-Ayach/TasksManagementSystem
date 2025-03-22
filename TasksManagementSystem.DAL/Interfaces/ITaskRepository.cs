using TasksManagementSystem.EF.Entities;

namespace TasksManagementSystem.DAL.Interfaces
{
    public interface ITaskRepository
    {
        Task<TaskEntity> InsertTask(TaskEntity entity);

        TaskEntity UpdateTask(TaskEntity entity);

        void DeleteTask(TaskEntity entity);

        Task<IEnumerable<TaskEntity>> GetAllTasksPagingByProjectId(int projectId, int skip, int take, string sortOrder);

        Task<IEnumerable<TaskEntity>> GetAllTasksForAnEmployeePagingByProjectId(string employeeId, int projectId, int skip, int take, string sortOrder);

        Task<IEnumerable<TaskEntity>> GetAllTasks();

        Task<TaskEntity?> GetTaskById(int id);
       
        Task<TaskEntity?> GetEmployeeTaskById(string employeeId, int id);

        Task<int> CountTasks();
       
        Task<int> CountTasksForAnEmployee(string employeeId);

        Task<int> CountTasksPerProject(int projectId);

        Task<int> CommitAsync();
    
    }
}
