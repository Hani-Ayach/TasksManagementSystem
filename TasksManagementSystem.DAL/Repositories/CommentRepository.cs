using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TasksManagementSystem.DAL.Interfaces;
using TasksManagementSystem.EF.DataContext;
using TasksManagementSystem.EF.Entities;

namespace TasksManagementSystem.DAL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CommentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
       
        async Task<CommentEntity> ICommentRepository.InsertComment(CommentEntity entity)
        {
            var t = await _dbContext.Comments.AddAsync(entity);
            return t.Entity;
        }
       
        CommentEntity ICommentRepository.UpdateComment(CommentEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("CommentEntity");

            var t = _dbContext.Comments.Update(entity);

            return t.Entity;
        }
       
        void ICommentRepository.DeleteComment(CommentEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("CommentEntity");

            _dbContext.Comments.Remove(entity);

        }
        
        async Task<IEnumerable<CommentEntity>> ICommentRepository.GetAllCommentsPaging(int skip, int take, string sortOrder)
        {
            return await _dbContext.Comments.OrderBy(sortOrder).Skip(skip).Take(take).ToListAsync();
        }
       
        async Task<IEnumerable<CommentEntity>> ICommentRepository.GetAllCommentsByTaskIdWithSorting(int taskId,string sortOrder)
        {
            return await _dbContext.Comments.Where(c=>c.TaskId==taskId).OrderBy(sortOrder).ToListAsync();
        }

        async Task<IEnumerable<CommentEntity>> ICommentRepository.GetAllComments()
        {
            return await _dbContext.Comments.ToListAsync();
        }

        async Task<CommentEntity?> ICommentRepository.GetCommentById(int id)
        {
            var set = _dbContext.Comments.Where(x => x.CommentId == id);


            return await set.SingleOrDefaultAsync();
        }

        async Task<int> ICommentRepository.CountComments()
        {
            return await _dbContext.Comments.CountAsync();
        }

        async Task<int> ICommentRepository.CommitAsync()
        {

            return await _dbContext.SaveChangesAsync();
        }
    
    }
}
