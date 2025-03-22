using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManagementSystem.EF.Entities
{
    public class ApplicationUserEntity:IdentityUser
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public List<ProjectEntity>? ProjectEntities { get; set; }

        public List<TaskEntity>? TaskEntities { get; set; }

        public List<CommentEntity>? CommentEntities { get; set; }


    }
}
