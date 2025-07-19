using System;
using System.ComponentModel.DataAnnotations;

namespace EFFramework.Models
{
    public class ProductStats
    {
        [Key]
        public int StatId { get; set; }

        [Required]
        public int TotalProducts { get; set; }

        [Required]
        public decimal AveragePrice { get; set; }

        [Required]
        public decimal TotalStockValue { get; set; }

        [Required]
        public int LowStockCount { get; set; }

        [Required]
        public int DiscontinuedCount { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }
    }
} 