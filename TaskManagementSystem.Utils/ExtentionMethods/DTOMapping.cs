using TaskManagementSystem.Utils.DTOs;
using TasksManagementSystem.EF.Entities;
using TaskStatus = TasksManagementSystem.EF.Entities.TaskStatus;

namespace TaskManagementSystem.Utils.ExtentionMethods
{
    public static class DTOMapping
    {
        public static ProjectEntity ToProjectEntity(this ProjectInputDTO projectInputDTO)
        {
            return new ProjectEntity() { ProjectId = projectInputDTO.Id, Name = projectInputDTO.Name, Description = projectInputDTO.Description, CreatedById = projectInputDTO.CreatedBy };
        }

        public static TaskEntity ToTaskEntity(this TaskInputDTO taskInputDTO)
        {
            return new TaskEntity() { TaskId = taskInputDTO.Id, ProjectId = taskInputDTO.ProjectId, Title = taskInputDTO.Title, Description = taskInputDTO.Description, CreatedById = taskInputDTO.CreatedBy, AssignedToId = taskInputDTO.AssignedTo, Status = taskInputDTO.Status == null ? TaskStatus.PENDING : (TaskStatus)taskInputDTO.Status };
        }

        public static CommentEntity ToCommentEntity(this CommentInputDTO commentInputDTO)
        {
            return new CommentEntity() { CommentId = commentInputDTO.Id, Text = commentInputDTO.Text, TaskId = commentInputDTO.TaskId, CreatedById = commentInputDTO.CreatedBy };
        }
   
    }
}
