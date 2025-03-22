using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagementSystem.Utils.DTOs;
using TasksManagementSystem.BL.Services;

namespace TasksManagementSystem.WebApp.Pages.Admin.Projects
{
    [Authorize(Roles = "ADMIN")]
    public class DisplayProjectModel : PageModel
    {
        private readonly ProjectServices _projectServices;
        private readonly ILogger<DisplayProjectModel> _logger;

        public DisplayProjectModel(ProjectServices projectServices, ILogger<DisplayProjectModel> logger)
        {
            _projectServices = projectServices;
            _logger = logger;
        }

        public ProjectDTO Project { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {

            try
            {
                Project = await this._projectServices.GetProjectById(id);

                if (Project == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting project");
                TempData["ErrorMessage"] = "An unexpected error occurred while getting the project.";
            }
            return Page();

        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {

                var result = await this._projectServices.DeleteProject(id);

                if (!!result)
                    TempData["SuccessMessage"] = "Project deleted successfully!";

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the project");
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the project.";
            }
            return RedirectToPage("Index");
        }
    }
}
