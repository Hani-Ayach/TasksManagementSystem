using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TaskManagementSystem.Utils.DTOs;
using TasksManagementSystem.BL.Services;
using TasksManagementSystem.EF.Entities;

namespace TasksManagementSystem.WebApp.Pages.Admin.Projects
{
    [Authorize(Roles = "ADMIN")]
    public class CreateProjectModel : PageModel
    {
        private readonly ProjectServices _projectServices;
        private readonly ILogger<CreateProjectModel> _logger;

        public CreateProjectModel(ProjectServices projectServices, ILogger<CreateProjectModel> logger)
        {
            _projectServices = projectServices;
            _logger = logger;
        }

        [BindProperty]
        public ProjectInputDTO Project { get; set; } = new ProjectInputDTO();
        public string UserId { get; set; }
        public void OnGet()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get User ID

            UserId = userId;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                var result = await _projectServices.CreateProject(Project);

                if (result != null)
                    TempData["SuccessMessage"] = "Project created successfully!";

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the project '{ProjectName}'.", Project.Name);
                TempData["ErrorMessage"] = "An unexpected error occurred while creating the project.";
                return Page();
            }
            return RedirectToPage("Index");
        }
    }
}
