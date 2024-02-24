using System.ComponentModel.DataAnnotations;

namespace DirWatcher.Models
{
    public class BgConfig
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string? Directory { get; set; }

        [Required]
        public int Interval { get; set; }

        [Required]
        public string? MagicString { get; set; }
    }
}
