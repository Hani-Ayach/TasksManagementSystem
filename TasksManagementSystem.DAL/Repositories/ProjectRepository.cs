using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TasksManagementSystem.DAL.Interfaces;
using TasksManagementSystem.EF.DataContext;
using TasksManagementSystem.EF.Entities;

namespace TasksManagementSystem.DAL.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        async Task<ProjectEntity> IProjectRepository.InsertProject(ProjectEntity entity)
        {
            var t = await _dbContext.Projects.AddAsync(entity);
            return t.Entity;
        }
        
        ProjectEntity IProjectRepository.UpdateProject(ProjectEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("ProjectEntity");

            var t = _dbContext.Projects.Update(entity);

            return t.Entity;
        }

        void IProjectRepository.DeleteProject(ProjectEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("ProjectEntity");

            _dbContext.Projects.Remove(entity);

        }
       
        async Task<IEnumerable<ProjectEntity>> IProjectRepository.GetAllProjectsPaging(int skip, int take, string sortOrder)
        {
            return await _dbContext.Projects.OrderBy(sortOrder).Skip(skip).Take(take).ToListAsync();
        }
     
        async Task<IEnumerable<ProjectEntity>> IProjectRepository.GetAllTasksAssignedToAnEmployeeGroupedByProjects(string employeeId)
        {
            return await _dbContext.Projects.Include(p => p.TaskEntities.Where(t => t.AssignedToId == employeeId)).Where(t => t.TaskEntities.Count() > 0).ToListAsync();

        }

        async Task<IEnumerable<ProjectEntity>> IProjectRepository.GetAllProjects()
        {
            return await _dbContext.Projects.ToListAsync();
        }

        async Task<ProjectEntity?> IProjectRepository.GetProjectById(int id)
        {
            var set = _dbContext.Projects.Where(x => x.ProjectId == id);


            return await set.SingleOrDefaultAsync();
        }

        async Task<int> IProjectRepository.CountProjects()
        {
            return await _dbContext.Projects.CountAsync();
        }

        async Task<int> IProjectRepository.CommitAsync()
        {

            return await _dbContext.SaveChangesAsync();
        }

    }
}
