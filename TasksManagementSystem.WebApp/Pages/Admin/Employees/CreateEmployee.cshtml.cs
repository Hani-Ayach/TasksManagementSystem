using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagementSystem.Utils.DTOs;
using TasksManagementSystem.BL.Services;

namespace TasksManagementSystem.WebApp.Pages.Admin.Projects.Employees
{
    [Authorize(Roles = "ADMIN")]
    public class AddEmployeeModel : PageModel
    {
        private readonly EmployeeServices _employeeServices;
        private readonly ILogger<AddEmployeeModel> _logger;

        public AddEmployeeModel(EmployeeServices employeeServices, ILogger<AddEmployeeModel> logger)
        {
            _employeeServices = employeeServices;
            _logger = logger;
        }

        [BindProperty]
        public EmployeeInputDTO Input { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                var result = await this._employeeServices.CreateEmployee(Input);

                if (!!result)
                    TempData["SuccessMessage"] = "Employee created successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the employee '{EmployeeName}'.", Input.Email);
                TempData["ErrorMessage"] = ex.Message;
                return Page();
            }

            return RedirectToPage("Index");
        }
  
    }
}
