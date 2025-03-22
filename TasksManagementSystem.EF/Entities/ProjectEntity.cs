using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TasksManagementSystem.EF.Entities
{
    public class ProjectEntity:BaseEntity
    {
        [Key]
        public int ProjectId { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string? CreatedById { get; set; }

        public List<TaskEntity>? TaskEntities { get; set; }
    }
}
