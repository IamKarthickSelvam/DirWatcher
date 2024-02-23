using System.ComponentModel.DataAnnotations;

namespace DirWatcher.Models
{
    public class BgConfig
    {
        [Key]
        public int Id { get; set; }
        public string? Directory { get; set; }
        public int Interval { get; set; }
        public string? MagicString { get; set; }
    }
}
