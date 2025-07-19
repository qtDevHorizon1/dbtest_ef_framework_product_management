using System;
using System.Collections.Generic;
using EFFramework.DataAccess;
using EFFramework.Models;

namespace EFFramework.Business
{
    public class ProductService
    {
        private readonly ProductRepository _repository;

        public ProductService()
        {
            _repository = new ProductRepository();
        }

        public List<Product> GetAllProducts()
        {
            return _repository.GetAllProducts();
        }

        public Product GetProduct(int productId)
        {
            return _repository.GetProductById(productId);
        }

        public int CreateProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Product name is required.");

            if (product.Price < 0)
                throw new ArgumentException("Price cannot be negative.");

            if (product.StockQuantity < 0)
                throw new ArgumentException("Stock quantity cannot be negative.");

            return _repository.InsertProduct(product);
        }

        public void UpdateProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Product name is required.");

            if (product.Price < 0)
                throw new ArgumentException("Price cannot be negative.");

            if (product.StockQuantity < 0)
                throw new ArgumentException("Stock quantity cannot be negative.");

            _repository.UpdateProduct(product);
        }

        public void DeleteProduct(int productId)
        {
            _repository.DeleteProduct(productId);
        }

        public void UpdateProductStock(int productId, int newQuantity)
        {
            if (newQuantity < 0)
                throw new ArgumentException("Stock quantity cannot be negative.");

            var product = _repository.GetProductById(productId);
            if (product == null)
                throw new ArgumentException($"Product with ID {productId} not found.");

            product.StockQuantity = newQuantity;
            _repository.UpdateProduct(product);
        }

        public void Dispose()
        {
            _repository?.Dispose();
        }
    }
} 