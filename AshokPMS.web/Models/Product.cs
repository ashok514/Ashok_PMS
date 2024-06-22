using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AshokPMS.web.Models
{
    public class Product : BaseEntity
    {
        [Required]
        [MaxLength(30)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Required]
        [MaxLength(250)]
        [Display(Name = "Product Description")]
        public string ProductDescription { get; set; }
        public string Manufacturer { get; set; }
        [Display(Name = "Product Date")]
        public DateTime ProductionDate { get; set; }
        [Display(Name = "Expiry Date")]
        public DateTime? ExpiryDate { get; set; }
        [Display(Name = "Batch No")]
        public string BatchNo { get; set; }
        public float Price { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public string ImageUrl { get; set; }
       

        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

    }
}
