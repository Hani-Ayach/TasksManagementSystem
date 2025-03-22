using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Utils.DTOs;
using TaskManagementSystem.Utils.ExtentionMethods;
using TasksManagementSystem.DAL.Interfaces;
using TasksManagementSystem.EF.Entities;

namespace TasksManagementSystem.BL.Services
{
    public class CommentServices
    {
        private ICommentRepository _comment;
        private ITaskRepository _task;
        private readonly UserManager<ApplicationUserEntity> _userManager;
       
        public CommentServices(ICommentRepository comment, ITaskRepository task, UserManager<ApplicationUserEntity> userManager)
        {
            this._comment = comment;
            this._task = task;
            this._userManager = userManager;
        }

        public async Task<CommentDTO> CreateComment(CommentInputDTO commentInputDTO)
        {
            if (commentInputDTO is null)
                throw new ArgumentNullException("CommentInputDTO", "CommentInputDTO is required");

            if (string.IsNullOrEmpty(commentInputDTO.CreatedBy))
                throw new ArgumentException("The creater id must not be null", "CreatedBy");

            if (commentInputDTO.TaskId <= 0)
                throw new ArgumentException("The task id must be greater than zero", "TaskId");

            var userCreator = await this._userManager.FindByIdAsync(commentInputDTO.CreatedBy);

            if (userCreator is null)
                throw new ArgumentNullException("CreatedBy", "There is no user with this id");

            var task = await this._task.GetTaskById(commentInputDTO.TaskId);

            if (task is null)
                throw new ArgumentNullException("TaskId", "There is no task with this id");


            var result = await this._comment.InsertComment(commentInputDTO.ToCommentEntity());

            await this._comment.CommitAsync();

            return result.ToCommentDTO();
        }
       
        public async Task<List<CommentDTO>> GetCommentsByTaskId(int taskId)
        {
            if (taskId <= 0)
                throw new ArgumentException("The task id must be greater than zero", "TaskId");

            var task = await this._task.GetTaskById(taskId);

            if (task is null)
                throw new ArgumentNullException("TaskId", "There is no task with this id");

            List<CommentDTO> response = new List<CommentDTO>();

            string sortOrder = "CreatedAt ASC";

            var result = await this._comment.GetAllCommentsByTaskIdWithSorting(taskId, sortOrder);

            // Fetch all user data in one query to avoid N+1 issue
            var userIds = result.Select(c => c.CreatedById).Distinct().ToList();
            var usersDict = (await _userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync())
                .ToDictionary(u => u.Id);

            response = result.Select(comment =>
           {
               var commentDTO = comment.ToCommentDTO();

               if (usersDict.TryGetValue(comment.CreatedById, out var userCreator))
               {
                   commentDTO.CreatedByName = $"{userCreator.FirstName} {userCreator.LastName}";
               }

               return commentDTO;
           }).ToList();

            return response;
        }

    }
}
