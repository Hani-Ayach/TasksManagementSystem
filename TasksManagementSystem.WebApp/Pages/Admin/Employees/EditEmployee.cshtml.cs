using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagementSystem.Utils.DTOs;
using TasksManagementSystem.BL.Services;

namespace TasksManagementSystem.WebApp.Pages.Admin.Projects.Employees
{
    [Authorize(Roles = "ADMIN")]
    public class EditEmployeeModel : PageModel
    {
        private readonly EmployeeServices _employeeServices;
        private readonly ILogger<EditEmployeeModel> _logger;

        public EditEmployeeModel(EmployeeServices employeeServices, ILogger<EditEmployeeModel> logger)
        {
            _employeeServices = employeeServices;
            _logger = logger;
        }

        [BindProperty]
        public EmployeeEditInputDTO Input { get; set; } = new EmployeeEditInputDTO();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            try
            {

                var employee = await this._employeeServices.GetEmployeeById(id);

                if (employee == null)
                {
                    return NotFound();
                }

                Input.Id = employee.Id;
                Input.FirstName = employee.FirstName;
                Input.LastName = employee.LastName;
                Input.Email = employee.Email;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the employee");
                TempData["ErrorMessage"] = "An unexpected error occurred while getting the employee.";
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

                var result = await this._employeeServices.EditEmployee(Input);

                if (!!result)
                    TempData["SuccessMessage"] = "Employee edited successfully!";

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing the employee '{EmployeeName}'.", Input.Email);
                TempData["ErrorMessage"] = ex.Message;
                return Page();
            }

            return RedirectToPage("Index");
        }

    }
}
