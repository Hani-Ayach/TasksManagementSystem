using Microsoft.AspNetCore.Identity;
using System.Linq.Dynamic.Core;
using TaskManagementSystem.Utils.DTOs;
using TasksManagementSystem.EF.Entities;

namespace TasksManagementSystem.BL.Services
{
    public class EmployeeServices
    {
        const string employeesROLE = "USER";
        private readonly UserManager<ApplicationUserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmployeeServices(UserManager<ApplicationUserEntity> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }
  
        public async Task<bool> CreateEmployee(EmployeeInputDTO employeeInputDTO)
        {
            if (employeeInputDTO is null)
                throw new ArgumentNullException("EmployeeInputDTO", "EmployeeInputDTO is required");

            var user = new ApplicationUserEntity
            {
                FirstName = employeeInputDTO.FirstName,
                LastName = employeeInputDTO.LastName,
                UserName = employeeInputDTO.Email,
                Email = employeeInputDTO.Email
            };

            var result = await _userManager.CreateAsync(user, employeeInputDTO.Password);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(employeesROLE))
                {
                    await _roleManager.CreateAsync(new IdentityRole(employeesROLE));
                }

                await _userManager.AddToRoleAsync(user, employeesROLE);
            }
            else
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new Exception($"User creation failed: {errors}");
            }
            return result.Succeeded;
        }
      
        public async Task<bool> EditEmployee(EmployeeEditInputDTO employeeInputDTO)
        {
            if (employeeInputDTO is null)
                throw new ArgumentNullException("EmployeeInputDTO", "EmployeeInputDTO is required");


            if (string.IsNullOrEmpty(employeeInputDTO.Id))
                throw new ArgumentException("The Employee id must not be null", "EmployeeId");

            var employee = await this._userManager.FindByIdAsync(employeeInputDTO.Id);

            if (employee is null)
                throw new ArgumentNullException("EmployeeId", "There is no employee with this id");


            employee.FirstName = employeeInputDTO.FirstName;
            employee.LastName = employeeInputDTO.LastName;
            employee.UserName = employeeInputDTO.Email;
            employee.Email = employeeInputDTO.Email;


            var result = await _userManager.UpdateAsync(employee);

            return result.Succeeded;
        }
      
        public async Task<bool> DeleteEmployee(string employeeId)
        {
            if (string.IsNullOrEmpty(employeeId))
                throw new ArgumentException("The Employee id must not be null", "EmployeeId");

            var employee = await this._userManager.FindByIdAsync(employeeId);

            if (employee is null)
                throw new ArgumentNullException("EmployeeId", "There is no employee with this id");


            var result = await _userManager.DeleteAsync(employee);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new Exception($"User creation failed: {errors}");
            }
            return result.Succeeded;
        }
       
        public async Task<List<EmployeeDTO>> GetEmployees()
        {
            List<EmployeeDTO> employees = new List<EmployeeDTO>();


            var userEntities = await _userManager.GetUsersInRoleAsync(employeesROLE);

            employees = userEntities.Select(userEntity => new EmployeeDTO
            {
                FullName = $"{userEntity.FirstName} {userEntity.LastName}",
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Email = userEntity.Email,
                Id = userEntity.Id
            }).ToList();

            return employees;
        }
      
        public async Task<EmployeeDTO> GetEmployeeById(string id)
        {

            EmployeeDTO employee = new EmployeeDTO();

            var userEntity = await _userManager.FindByIdAsync(id);

            employee.FullName = $"{userEntity.FirstName} {userEntity.LastName}";
            employee.FirstName = userEntity.FirstName;
            employee.LastName = userEntity.LastName;
            employee.Email = userEntity.Email;
            employee.Id = userEntity.Id;

            return employee;
        }

    }
}
