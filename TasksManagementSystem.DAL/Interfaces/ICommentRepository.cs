using TasksManagementSystem.EF.Entities;

namespace TasksManagementSystem.DAL.Interfaces
{
    public interface ICommentRepository
    {
        Task<CommentEntity> InsertComment(CommentEntity entity);

        CommentEntity UpdateComment(CommentEntity entity);

        void DeleteComment(CommentEntity entity);
      
        Task<IEnumerable<CommentEntity>> GetAllCommentsPaging(int skip, int take, string sortOrder);

        Task<IEnumerable<CommentEntity>> GetAllCommentsByTaskIdWithSorting(int taskId, string sortOrder);

        Task<IEnumerable<CommentEntity>> GetAllComments();

        Task<CommentEntity?> GetCommentById(int id);

        Task<int> CountComments();

        Task<int> CommitAsync();

    }
}
