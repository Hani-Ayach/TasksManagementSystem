using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TasksManagementSystem.DAL.Interfaces;
using TasksManagementSystem.EF.DataContext;
using TasksManagementSystem.EF.Entities;

namespace TasksManagementSystem.DAL.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TaskRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    
        async Task<TaskEntity> ITaskRepository.InsertTask(TaskEntity entity)
        {
            var t = await _dbContext.Tasks.AddAsync(entity);
            return t.Entity;
        }
    
        TaskEntity ITaskRepository.UpdateTask(TaskEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("TaskEntity");

            var t = _dbContext.Tasks.Update(entity);

            return t.Entity;
        }
    
        void ITaskRepository.DeleteTask(TaskEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("TaskEntity");

            _dbContext.Tasks.Remove(entity);

        }
     
        async Task<IEnumerable<TaskEntity>> ITaskRepository.GetAllTasksPagingByProjectId(int projectId, int skip, int take, string sortOrder)
        {
            return await _dbContext.Tasks.Where(t => t.ProjectId == projectId).OrderBy(sortOrder).Skip(skip).Take(take).ToListAsync();
        }

        async Task<IEnumerable<TaskEntity>> ITaskRepository.GetAllTasksForAnEmployeePagingByProjectId(string employeeId, int projectId, int skip, int take, string sortOrder)
        {
            return await _dbContext.Tasks.Where(t => t.ProjectId == projectId && t.AssignedToId == employeeId).OrderBy(sortOrder).Skip(skip).Take(take).ToListAsync();
        }
      
        async Task<IEnumerable<TaskEntity>> ITaskRepository.GetAllTasks()
        {
            return await _dbContext.Tasks.ToListAsync();
        }

        async Task<TaskEntity?> ITaskRepository.GetTaskById(int id)
        {
            var set = _dbContext.Tasks.Where(x => x.TaskId == id);


            return await set.SingleOrDefaultAsync();
        }
      
        async Task<TaskEntity?> ITaskRepository.GetEmployeeTaskById(string employeeId, int id)
        {
            var set = _dbContext.Tasks.Where(x => x.TaskId == id && x.AssignedToId == employeeId);

            return await set.SingleOrDefaultAsync();
        }
      
        async Task<int> ITaskRepository.CountTasks()
        {
            return await _dbContext.Tasks.CountAsync();
        }

        async Task<int> ITaskRepository.CountTasksForAnEmployee(string employeeId)
        {
            return await _dbContext.Tasks.Where(t => t.AssignedToId == employeeId).CountAsync();
        }
      
        async Task<int> ITaskRepository.CountTasksPerProject(int projectId)
        {
            return await _dbContext.Tasks.Where(t => t.ProjectId == projectId).CountAsync();
        }
       
        async Task<int> ITaskRepository.CommitAsync()
        {

            return await _dbContext.SaveChangesAsync();
        }
  
    }
}
