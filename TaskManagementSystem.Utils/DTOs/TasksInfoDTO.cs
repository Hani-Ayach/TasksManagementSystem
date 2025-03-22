namespace TaskManagementSystem.Utils.DTOs
{
    public class TasksInfoDTO
    {
        public List<TaskDTO> Tasks { get; set; }
        public int TasksCount { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}
