using System.ComponentModel.DataAnnotations;

namespace MyService.Entities
{
    public class Department
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
