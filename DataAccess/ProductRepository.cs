using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EFFramework.Models;

namespace EFFramework.DataAccess
{
    public class ProductRepository : IDisposable
    {
        private readonly ProductDbContext _context;

        public ProductRepository()
        {
            _context = new ProductDbContext();
            // Enable logging to see SQL queries
            _context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        public List<Product> GetAllProducts()
        {
            try
            {
                return _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Supplier)
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting all products: {ex.Message}");
                throw;
            }
        }

        public Product GetProductById(int productId)
        {
            try
            {
                return _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Supplier)
                    .FirstOrDefault(p => p.ProductId == productId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting product by ID {productId}: {ex.Message}");
                throw;
            }
        }

        public List<Product> GetProductsByCategory(int categoryId)
        {
            try
            {
                return _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Supplier)
                    .Where(p => p.CategoryId == categoryId)
                    .OrderBy(p => p.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting products by category {categoryId}: {ex.Message}");
                throw;
            }
        }

        public List<Product> GetProductsBySupplier(int supplierId)
        {
            try
            {
                return _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Supplier)
                    .Where(p => p.SupplierId == supplierId)
                    .OrderBy(p => p.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting products by supplier {supplierId}: {ex.Message}");
                throw;
            }
        }

        public List<Product> GetLowStockProducts()
        {
            try
            {
                return _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Supplier)
                    .Where(p => p.StockQuantity <= p.ReorderLevel && !p.IsDiscontinued)
                    .OrderBy(p => p.StockQuantity)
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting low stock products: {ex.Message}");
                throw;
            }
        }

        public List<Product> SearchProducts(string searchTerm)
        {
            try
            {
                return _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Supplier)
                    .Where(p => p.Name.Contains(searchTerm) || 
                               p.Description.Contains(searchTerm) || 
                               p.SKU.Contains(searchTerm))
                    .OrderBy(p => p.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error searching products: {ex.Message}");
                throw;
            }
        }

        public List<Product> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            try
            {
                return _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Supplier)
                    .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                    .OrderBy(p => p.Price)
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting products by price range: {ex.Message}");
                throw;
            }
        }

        public List<ProductHistory> GetProductHistory(int productId)
        {
            try
            {
                return _context.ProductHistory
                    .Where(h => h.ProductId == productId)
                    .OrderByDescending(h => h.ActionDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting product history for {productId}: {ex.Message}");
                throw;
            }
        }

        public ProductStats GetProductStats()
        {
            try
            {
                return _context.ProductStats.FirstOrDefault();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting product stats: {ex.Message}");
                throw;
            }
        }

        public List<Category> GetCategoryHierarchy()
        {
            try
            {
                return _context.Categories
                    .Where(c => c.ParentCategoryId == null)
                    .Include(c => c.SubCategories)
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting category hierarchy: {ex.Message}");
                throw;
            }
        }

        public int InsertProduct(Product product)
        {
            try
            {
                product.CreatedDate = DateTime.Now;
                _context.Products.Add(product);
                _context.SaveChanges();
                return product.ProductId;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error inserting product: {ex.Message}");
                throw;
            }
        }

        public void UpdateProduct(Product product)
        {
            try
            {
                var existingProduct = _context.Products.Find(product.ProductId);
                if (existingProduct != null)
                {
                    // Track changes for history
                    var oldPrice = existingProduct.Price;
                    var oldStock = existingProduct.StockQuantity;

                    _context.Entry(existingProduct).CurrentValues.SetValues(product);
                    existingProduct.ModifiedDate = DateTime.Now;

                    // Create history record if price or stock changed
                    if (oldPrice != product.Price || oldStock != product.StockQuantity)
                    {
                        var history = new ProductHistory
                        {
                            ProductId = product.ProductId,
                            Action = "UPDATE",
                            OldPrice = oldPrice,
                            NewPrice = product.Price,
                            OldStock = oldStock,
                            NewStock = product.StockQuantity,
                            ActionDate = DateTime.Now,
                            ModifiedBy = "System" // This should be replaced with actual user info
                        };
                        _context.ProductHistory.Add(history);
                    }

                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating product {product.ProductId}: {ex.Message}");
                throw;
            }
        }

        public void DeleteProduct(int productId)
        {
            try
            {
                var product = _context.Products.Find(productId);
                if (product != null)
                {
                    // Create history record before deletion
                    var history = new ProductHistory
                    {
                        ProductId = productId,
                        Action = "DELETE",
                        OldPrice = product.Price,
                        OldStock = product.StockQuantity,
                        ActionDate = DateTime.Now,
                        ModifiedBy = "System" // This should be replaced with actual user info
                    };
                    _context.ProductHistory.Add(history);

                    _context.Products.Remove(product);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting product {productId}: {ex.Message}");
                throw;
            }
        }

        public void ExecuteInTransaction(Action action)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    action();
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
} 