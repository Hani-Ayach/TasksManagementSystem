using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TaskManagementSystem.Utils.DTOs;
using TasksManagementSystem.BL.Services;

namespace TasksManagementSystem.WebApp.Pages.Employee.Tasks
{
    [Authorize(Roles = "USER")]
    public class DisplayTaskModel : PageModel
    {

        private readonly TaskServices _task;
        private readonly CommentServices _comment;
        private readonly ILogger<DisplayTaskModel> _logger;

        public DisplayTaskModel(ILogger<DisplayTaskModel> logger, TaskServices task, CommentServices comment)
        {
            _task = task;
            _comment = comment;
            _logger = logger;
        }

        public TaskDTO Task { get; set; }
        public List<CommentDTO> Comments { get; set; }
        [BindProperty] public CommentInputDTO NewComment { get; set; }
        public string UserId { get; set; }
        public int TaskId { get; set; }

        public string NotApproved = "NOT_APPROVED";

        public string Pending = "PENDING";

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Task = await this._task.GetEmployeeTaskById(employeeId, id);

            if (Task == null)
            {
                return NotFound();
            }

            Comments = await _comment.GetCommentsByTaskId(id);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get User ID

            UserId = userId;

            TaskId = id;

            return Page();
        }

        public async Task<IActionResult> OnPostAddCommentAsync(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return await OnGetAsync(id);
                }
                var result = await this._comment.CreateComment(NewComment);

                if (result != null)
                    TempData["SuccessMessage"] = "Comment created successfully!";

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while posting the comment");
                TempData["ErrorMessage"] = "An unexpected error occurred while posted the comment.";
            }
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostCompleteAsync(int id)
        {
            try
            {
                var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await this._task.MakeTaskCompletedByEmployee(employeeId, id);

                if (result != null)
                    TempData["SuccessMessage"] = "Task completed successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while making the task completed");
                TempData["ErrorMessage"] = "An unexpected error occurred while making the task completed.";
            }
            return RedirectToPage(new { id });

        }

        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            try
            {
                var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await this._task.AppoveTaskByEmployee(employeeId, id);

                if (result != null)
                    TempData["SuccessMessage"] = "Task approved successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while approving the task");
                TempData["ErrorMessage"] = "An unexpected error occurred while approving the task.";
            }
            return RedirectToPage(new { id });

        }

    }
}
