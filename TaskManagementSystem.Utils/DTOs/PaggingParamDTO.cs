namespace TaskManagementSystem.Utils.DTOs
{
    public class PaggingParamDTO
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public string OrderBy { get; set; }
        public string SortOrder { get; set; }
    }
}
