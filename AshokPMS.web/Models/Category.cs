using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AshokPMS.web.Models
{
    public class Category : BaseEntity
    {
        [Required]
        [MaxLength(30)]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Category Description")]
        public string CategoryDescription { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

       // public virtual ICollection<Product> ProductInfos { get; set; }
    }
}
