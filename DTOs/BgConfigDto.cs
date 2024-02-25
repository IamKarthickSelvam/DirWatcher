using System.ComponentModel.DataAnnotations;

namespace DirWatcher.DTOs
{
    public class BgConfigDto
    {
        [Required]
        public string? Directory { get; set; }

        [Required]
        public int Interval { get; set; }

        [Required]
        public string? MagicString { get; set; }
    }
}