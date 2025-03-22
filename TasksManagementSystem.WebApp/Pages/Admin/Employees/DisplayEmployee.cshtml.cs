using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagementSystem.Utils.DTOs;
using TasksManagementSystem.BL.Services;

namespace TasksManagementSystem.WebApp.Pages.Admin.Projects.Employees
{
    [Authorize(Roles = "ADMIN")]
    public class DisplayEmployeeModel : PageModel
    {
        private readonly EmployeeServices _employeeServices;
        private readonly ILogger<DisplayEmployeeModel> _logger;

        public DisplayEmployeeModel(EmployeeServices employeeServices, ILogger<DisplayEmployeeModel> logger)
        {
            _employeeServices = employeeServices;
            _logger = logger;
        }

        [BindProperty]
        public EmployeeDTO Employee { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                Employee = await _employeeServices.GetEmployeeById(id);

                if (Employee == null)
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the employee '{EmployeeName}'.", Employee.FullName);
                TempData["ErrorMessage"] = "An unexpected error occurred while getting the employee.";
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            try
            {
                var result = await this._employeeServices.DeleteEmployee(id);

                if (!!result)
                    TempData["SuccessMessage"] = "Employee edited successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing the employee '{EmployeeName}'.", Employee.Email);
                TempData["ErrorMessage"] = "An unexpected error occurred while editing the employee.";
            }

            return RedirectToPage("Index");
        }
    
    }
}
