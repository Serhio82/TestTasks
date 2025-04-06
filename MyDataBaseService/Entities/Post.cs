

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyDataBaseService.Entities
{
    public class Post
    {
        [Key] 
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey("PromotionPost")] 
        public int? PromotionId { get; set; }

        public Post PromotionPost { get; set; } 

        [Required]
        public int DepartmentId { get; set; }

        [ForeignKey("Department")] 
        public Department Department { get; set; }

        public ICollection<Employee> Employees { get; set; }

    }
}
