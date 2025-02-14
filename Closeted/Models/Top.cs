using System.ComponentModel.DataAnnotations;

namespace Closeted.Models
{
    public class Top
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
