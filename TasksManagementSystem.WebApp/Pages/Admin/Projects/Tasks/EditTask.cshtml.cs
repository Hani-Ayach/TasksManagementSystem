using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagementSystem.Utils.DTOs;
using TasksManagementSystem.BL.Services;

namespace TasksManagementSystem.WebApp.Pages.Admin.Projects.Tasks
{
    [Authorize(Roles = "ADMIN")]
    public class EditTaskModel : PageModel
    {
        private readonly TaskServices _taskServices;
        private readonly EmployeeServices _employeeServices;
        private readonly ILogger<EditTaskModel> _logger;

        public EditTaskModel(TaskServices taskServices, EmployeeServices employeeServices, ILogger<EditTaskModel> logger)
        {
            _taskServices = taskServices;
            _employeeServices = employeeServices;
            _logger = logger;
        }

        [BindProperty]
        public TaskInputDTO Task { get; set; } = new TaskInputDTO();

        public List<EmployeeDTO> Employees { get; set; }
        public string UserId { get; set; }
        public int ProjectId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var task = await this._taskServices.GetTaskById(id);

                if (task == null)
                {
                    return NotFound();
                }

                Task.Title = task.Title;
                Task.ProjectId = task.ProjectId;
                Task.Description = task.Description;
                Task.AssignedTo = task.AssignedToId;
                Task.Status = task.Status;
                Task.CreatedBy = task.CreatedById;
                Task.Id = task.Id;

                Employees = await this._employeeServices.GetEmployees();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting employees");
            }
            return Page();
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

                var result = await _taskServices.EditTask(Task);

                if (result != null)
                    TempData["SuccessMessage"] = "Task updated successfully!";

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
