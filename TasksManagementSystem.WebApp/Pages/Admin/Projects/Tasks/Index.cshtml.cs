using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagementSystem.Utils.DTOs;
using TasksManagementSystem.BL.Services;

namespace TasksManagementSystem.WebApp.Pages.Admin.Projects.Tasks
{
    [Authorize(Roles = "ADMIN")]
    public class IndexModel : PageModel
    {
        private readonly TaskServices _taskServices;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(TaskServices taskServices, ILogger<IndexModel> logger)
        {
            _taskServices = taskServices;
            _logger = logger;
        }
        public TasksInfoDTO TasksInfo { get; set; } = new();
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public string SortOrder { get; set; }

        private const int PageSize = 5; // Number of tasks per page

        public async Task OnGetAsync(int projectId, int pageIndex = 1, string sortOrder = "desc")
        {
            try
            {
                SortOrder = sortOrder;
                PageIndex = pageIndex;
                PaggingParamDTO paggingParam = new PaggingParamDTO() { Skip = (PageIndex - 1) * PageSize, Take = PageSize, OrderBy = "CreatedAt", SortOrder = sortOrder };

                TasksInfo = await _taskServices.GetTasksByProjectId(projectId, paggingParam);

                TotalPages = (int)Math.Ceiling(TasksInfo.TasksCount / (double)PageSize);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting tasks");
                TempData["ErrorMessage"] = "An unexpected error occurred while getting the tasks.";
            }
        }
   
    }
}
