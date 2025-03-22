using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagementSystem.Utils.DTOs;
using TasksManagementSystem.BL.Services;

namespace TasksManagementSystem.WebApp.Pages.Admin.Projects.o
{
    [Authorize(Roles = "ADMIN")]
    public class IndexModel : PageModel
    {
        private readonly ProjectServices _projectServices;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ProjectServices projectServices, ILogger<IndexModel> logger)
        {
            _projectServices = projectServices;
            _logger = logger;
        }
        public ProjectsInfoDTO ProjectsInfo { get; set; } = new();
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public string SortOrder { get; set; }

        private const int PageSize = 5; // Number of projects per page

        public async Task OnGetAsync(int pageIndex = 1, string sortOrder = "desc")
        {
            try
            {
                SortOrder = sortOrder;
                PageIndex = pageIndex;
                PaggingParamDTO paggingParam = new PaggingParamDTO() { Skip = (PageIndex - 1) * PageSize, Take = PageSize, OrderBy = "CreatedAt", SortOrder = sortOrder };

                ProjectsInfo = await _projectServices.GetProjects(paggingParam);

                TotalPages = (int)Math.Ceiling(ProjectsInfo.ProjectsCount / (double)PageSize);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting projects");
                TempData["ErrorMessage"] = "An unexpected error occurred while getting the projects.";
            }
        }
    }
}
