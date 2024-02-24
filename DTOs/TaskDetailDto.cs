namespace DirWatcher.DTOs
{
    public class TaskDetailDto
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string? Status { get; set; }

        public List<string>? CurrentFiles { get; set; }

        public List<string>? AddedFiles { get; set; }

        public List<string>? DeletedFiles { get; set; }
    }
}
