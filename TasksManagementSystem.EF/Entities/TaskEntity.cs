using System.ComponentModel.DataAnnotations;

namespace TasksManagementSystem.EF.Entities
{
    public enum TaskStatus
    {
        PENDING,
        COMPLETED,
        APPROVED,
        NOT_APPROVED
    }
    public class TaskEntity:BaseEntity
    {
        [Key]
        public int TaskId { get; set; }

        [Required, MaxLength(150)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public TaskStatus? Status { get; set; }

        public bool? CanStatusSetApprovedByUser { get; set; }

        public int? ProjectId { get; set; }

        public string? AssignedToId { get; set; }

        public string? CreatedById { get; set; }

        public List<CommentEntity>? CommentEntities { get; set; }
    }
}
