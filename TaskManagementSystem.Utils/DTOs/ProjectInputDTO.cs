using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Utils.DTOs
{
    public class ProjectInputDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Project name is required.")]
        [StringLength(150, ErrorMessage = "Project name must be less than 150 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Project description is required.")]
        public string Description { get; set; }
        
        public string CreatedBy { get; set; }
    }
}
