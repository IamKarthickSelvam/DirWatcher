namespace DirWatcher.DTOs
{
    public class TaskDetailDto
    {
        public int TaskNo { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string? Status { get; set; }

        public TimeSpan TotalRunTime { get; set; }

        public int MagicCount { get; set; }

        public List<string>? CurrentFiles { get; set; }

        public List<string>? AddedFiles { get; set; }

        public List<string>? DeletedFiles { get; set; }
    }
}
