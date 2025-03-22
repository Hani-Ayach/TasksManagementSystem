using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TaskManagementSystem.Utils.DTOs;
using TasksManagementSystem.BL.Services;

namespace TasksManagementSystem.WebApp.Pages.Admin.Projects.Tasks
{
    [Authorize(Roles = "ADMIN")]
    public class CreateTaskModel : PageModel
    {
        private readonly TaskServices _taskServices;
        private readonly EmployeeServices _employeeServices;
        private readonly ILogger<CreateTaskModel> _logger;

        public CreateTaskModel(TaskServices taskServices, EmployeeServices employeeServices, ILogger<CreateTaskModel> logger)
        {
            _taskServices = taskServices;
            _employeeServices = employeeServices;
            _logger = logger;
        }

        [BindProperty]
        public TaskInputDTO Task { get; set; }
        public List<EmployeeDTO> Employees { get; set; }
        public string UserId { get; set; }
        public int ProjectId { get; set; }

        public async Task OnGetAsync(int projectId)
        {
            try
            {
                Employees = await this._employeeServices.GetEmployees();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get User ID

                UserId = userId;

                ProjectId = projectId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting employees");
            }
        }

        public async Task<IActionResult> OnPostAsync(int projectId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await OnGetAsync(projectId); // Reload Employees list
                    return Page();
                }

                var result = await _taskServices.CreateTask(Task);

                if (result != null)
                    TempData["SuccessMessage"] = "Task created successfully!";

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the task '{TaskName}'.", Task.Title);
                TempData["ErrorMessage"] = "An unexpected error occurred while creating the project.";
                return Page();
            }
            return RedirectToPage("Index", new { projectId = projectId }); // Redirect to Tasks List
        }
   
    }
}
