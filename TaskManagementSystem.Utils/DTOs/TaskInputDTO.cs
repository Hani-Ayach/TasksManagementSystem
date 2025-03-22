using System.ComponentModel.DataAnnotations;
using TaskStatus = TasksManagementSystem.EF.Entities.TaskStatus;

namespace TaskManagementSystem.Utils.DTOs
{
    public class TaskInputDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Task title is required.")]
        [StringLength(150, ErrorMessage = "Task title must be less than 150 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Task description is required.")]
        public string Description { get; set; }

        public TaskStatus Status { get; set; }

        [Required(ErrorMessage = "Task assigned is required.")]
        public string AssignedTo { get; set; }
     
        public string CreatedBy { get; set; }
      
        public int ProjectId { get; set; }

    }
}
