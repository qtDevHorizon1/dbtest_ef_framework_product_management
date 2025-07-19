using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFFramework.Models
{
    public class ProductHistory
    {
        [Key]
        public int HistoryId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [StringLength(10)]
        public string Action { get; set; }

        public decimal? OldPrice { get; set; }
        public decimal? NewPrice { get; set; }
        public int? OldStock { get; set; }
        public int? NewStock { get; set; }

        [Required]
        public DateTime ActionDate { get; set; }

        [StringLength(100)]
        public string ModifiedBy { get; set; }

        // Navigation property
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
} 