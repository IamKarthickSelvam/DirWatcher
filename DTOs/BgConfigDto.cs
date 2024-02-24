namespace DirWatcher.DTOs
{
    public class BgConfigDto
    {
        public string? Directory { get; set; }

        public int Interval { get; set; }

        public string? MagicString { get; set; }
    }
}