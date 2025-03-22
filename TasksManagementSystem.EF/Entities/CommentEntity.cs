using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManagementSystem.EF.Entities
{
    public class CommentEntity:BaseEntity
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        public string Text { get; set; }

        public int? TaskId { get; set; }

        public string? CreatedById { get; set; }

    }
}
