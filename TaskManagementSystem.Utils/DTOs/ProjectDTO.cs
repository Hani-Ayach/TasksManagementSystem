namespace TaskManagementSystem.Utils.DTOs
{
    public class ProjectDTO
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedByEmail { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TasksCount { get; set; }

    }
}
