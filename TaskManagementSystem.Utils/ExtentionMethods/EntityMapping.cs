using TaskManagementSystem.Utils.DTOs;
using TasksManagementSystem.EF.Entities;
using TaskStatus = TasksManagementSystem.EF.Entities.TaskStatus;

namespace TaskManagementSystem.Utils.ExtentionMethods
{
    public static class EntityMapping
    {
        public static ProjectDTO ToProjectDTO(this ProjectEntity projectEntity)
        {
            return new ProjectDTO() { ProjectId = projectEntity.ProjectId, Name = projectEntity.Name, Description = projectEntity.Description, CreatedAt = projectEntity.CreatedAt, CreatedById = projectEntity.CreatedById, TasksCount = 0 };
        }
        
        public static TaskDTO ToTaskDTO(this TaskEntity taskEntity)
        {
            return new TaskDTO() { Id = taskEntity.TaskId, Title = taskEntity.Title, Description = taskEntity.Description, CreatedById = taskEntity.CreatedById, AssignedToId = taskEntity.AssignedToId, Status = (TaskStatus)taskEntity.Status, CreatedAt = taskEntity.CreatedAt };
        }

        public static CommentDTO ToCommentDTO(this CommentEntity commentEntity)
        {
            return new CommentDTO() { Id = commentEntity.CommentId, Content = commentEntity.Text, CreatedDate = commentEntity.CreatedAt, TaskId = (int)commentEntity.TaskId };
        }
    }
}
