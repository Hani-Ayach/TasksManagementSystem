using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace TasksManagementSystem.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        const string AdminRole = "ADMIN";
        const string EmployeeRole = "USER";
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get User ID

            if (userId == null)
                return Redirect("Identity/Account/Login");

            if (User.IsInRole(AdminRole))
                return Redirect("/Admin/Projects/Index");

            return Redirect("/Employee/Index");
        }
    }
}
