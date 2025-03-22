using TaskStatus = TasksManagementSystem.EF.Entities.TaskStatus;

namespace TaskManagementSystem.Utils.DTOs
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedByEmail { get; set; }
        public string AssignedToId { get; set; }
        public string AssignedToName { get; set; }
        public string AssignedToEmail { get; set; }
        public int ProjectId { get; set; }
    }
}
