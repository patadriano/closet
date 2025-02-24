using System.ComponentModel.DataAnnotations;

namespace Closeted.Models
{
    public class Headwear
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public byte[] Image { get; set; }
    }
}
