using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TaskManagementSystem.Utils.DTOs;
using TasksManagementSystem.BL.Services;

namespace TasksManagementSystem.WebApp.Pages.Employee
{
    [Authorize(Roles ="USER")]
    public class IndexModel : PageModel
    {
        private readonly ProjectServices _project;
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(ILogger<IndexModel> logger, ProjectServices project)
        {
            _logger = logger;
            _project = project;
        }

        public List<ProjectDTO> Projects { get; set; } = new List<ProjectDTO>();
        public bool ShowFullDescripton { get; set; } = false;
        public async Task OnGetAsync()
        {
            try
            {
                var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                Projects = await this._project.GroupAssignedTasksByProjects(employeeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting tasks grouped by projects");
                TempData["ErrorMessage"] = "An unexpected error occurred while getting tasks grouped by projects";
            }
        }
    }
}

