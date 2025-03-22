using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TasksManagementSystem.EF.Entities;

namespace TasksManagementSystem.EF
{
    public static class ApplicationDbInitializer
    {
        const string adminRoleName = "ADMIN";
        const string userRoleName = "USER";

        public static void SeedRoles(this ModelBuilder builder)
        {
            string ADMIN_ROLE_ID = "341743f0-asd2–42de-afbf-59kmkkmk72cf6";
            string USER_ROLE_ID = "341743f0-asd2–42de-afbf-59dmkkmk72cf6";

            //seed roles
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = adminRoleName,
                NormalizedName = adminRoleName,
                Id = ADMIN_ROLE_ID,
                ConcurrencyStamp = ADMIN_ROLE_ID
            });

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = userRoleName,
                NormalizedName = userRoleName,
                Id = USER_ROLE_ID,
                ConcurrencyStamp = USER_ROLE_ID
            });
           

        }
    }

}

