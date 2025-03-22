using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagementSystem.Utils.DTOs;
using TasksManagementSystem.BL.Services;

namespace TasksManagementSystem.WebApp.Pages.Admin.Employees
{
    [Authorize(Roles = "ADMIN")]
    public class IndexModel : PageModel
    {
        private readonly EmployeeServices _employeeServices;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(EmployeeServices employeeServices, ILogger<IndexModel> logger)
        {
            _employeeServices = employeeServices;
            _logger = logger;
        }
        public List<EmployeeDTO> Employees { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                Employees = await _employeeServices.GetEmployees();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting employees");
                TempData["ErrorMessage"] = "An unexpected error occurred while getting the employees.";
            }
        }
    }
}
