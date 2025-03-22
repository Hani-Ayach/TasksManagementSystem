using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Utils.DTOs
{
    public class CommentInputDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Comment cannot be empty")]
        public string Text { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public int TaskId { get; set; }
    }
}
