using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TaskManagementSystem.Utils.DTOs;
using TasksManagementSystem.BL.Services;

namespace TasksManagementSystem.WebApp.Pages.Admin.Projects
{
    [Authorize(Roles = "ADMIN")]
    public class EditProjectModel : PageModel
    {
        private readonly ProjectServices _projectServices;
        private readonly ILogger<EditProjectModel> _logger;

        public EditProjectModel(ProjectServices projectServices, ILogger<EditProjectModel> logger)
        {
            _projectServices = projectServices;
            _logger = logger;
        }

        [BindProperty]
        public ProjectInputDTO Project { get; set; } = new ProjectInputDTO();
        public string UserId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {

                var project = await this._projectServices.GetProjectById(id);

                if (project == null)
                {
                    return NotFound();
                }

                Project.Name = project.Name;
                Project.Description = project.Description;
                Project.Id = project.ProjectId;

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get User ID

                UserId = userId;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting project");
                TempData["ErrorMessage"] = "An unexpected error occurred while getting the project.";
            }
            return Page();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

               var result= await this._projectServices.EditProject(Project);

                if (result != null)
                    TempData["SuccessMessage"] = "Project updated successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing project");
                TempData["ErrorMessage"] = "An unexpected error occurred while editing the project.";
            }
            return RedirectToPage("Index");
        }
    }
}
