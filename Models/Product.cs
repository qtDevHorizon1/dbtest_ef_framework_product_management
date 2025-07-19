using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace EFFramework.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }

        [StringLength(50)]
        public string SKU { get; set; }

        public decimal? Weight { get; set; }

        [StringLength(50)]
        public string Dimensions { get; set; }

        [Required]
        public bool IsDiscontinued { get; set; }

        [Required]
        public int ReorderLevel { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

        public virtual ICollection<ProductHistory> History { get; set; }

        public override string ToString()
        {
            return $"Product ID: {ProductId}\n" +
                   $"Name: {Name}\n" +
                   $"Description: {Description}\n" +
                   $"Price: ${Price:N2}\n" +
                   $"Stock: {StockQuantity}\n" +
                   $"Created: {CreatedDate:g}\n" +
                   $"Modified: {(ModifiedDate.HasValue ? ModifiedDate.Value.ToString("g") : "Not modified")}";
        }
    }
} 