using System;
using System.Collections.Generic;
using EFFramework.Business;
using EFFramework.Models;
using EFFramework.CLI;

namespace EFFramework
{
    class Program
    {
        private static readonly ProductService _productService = new ProductService();

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                // CLI mode
                var cli = new CommandLineInterface();
                cli.ProcessCommand(args);
                return;
            }

            // Interactive mode
            RunInteractiveMode();
        }

        private static void RunInteractiveMode()
        {
            while (true)
            {
                try
                {
                    ShowMenu();
                    var choice = Console.ReadLine();

                    switch (choice?.ToLower())
                    {
                        case "1":
                            ListAllProducts();
                            break;
                        case "2":
                            GetProductById();
                            break;
                        case "3":
                            CreateNewProduct();
                            break;
                        case "4":
                            UpdateExistingProduct();
                            break;
                        case "5":
                            DeleteExistingProduct();
                            break;
                        case "6":
                            UpdateProductStock();
                            break;
                        case "q":
                            return;
                        default:
                            Console.WriteLine("\nInvalid choice. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Details: {ex.InnerException.Message}");
                    }
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private static void ShowMenu()
        {
            Console.WriteLine("Product Management System");
            Console.WriteLine("------------------------");
            Console.WriteLine("1. List all products");
            Console.WriteLine("2. Get product by ID");
            Console.WriteLine("3. Create new product");
            Console.WriteLine("4. Update product");
            Console.WriteLine("5. Delete product");
            Console.WriteLine("6. Update product stock");
            Console.WriteLine("Q. Quit");
            Console.Write("\nEnter your choice: ");
        }

        private static void ListAllProducts()
        {
            var products = _productService.GetAllProducts();
            DisplayProducts(products);
        }

        private static void GetProductById()
        {
            Console.Write("\nEnter product ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var product = _productService.GetProduct(id);
                Console.WriteLine("\n" + product.ToString());
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }

        private static void CreateNewProduct()
        {
            var product = GetProductDetailsFromUser();
            var newId = _productService.CreateProduct(product);
            Console.WriteLine($"\nProduct created successfully with ID: {newId}");
        }

        private static void UpdateExistingProduct()
        {
            Console.Write("\nEnter product ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var product = _productService.GetProduct(id);
                var updatedProduct = GetProductDetailsFromUser();
                updatedProduct.ProductId = id;
                _productService.UpdateProduct(updatedProduct);
                Console.WriteLine("Product updated successfully.");
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }

        private static void DeleteExistingProduct()
        {
            Console.Write("\nEnter product ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                _productService.DeleteProduct(id);
                Console.WriteLine("Product deleted successfully.");
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }

        private static void UpdateProductStock()
        {
            Console.Write("\nEnter product ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID format.");
                return;
            }

            Console.Write("Enter new stock quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity))
            {
                Console.WriteLine("Invalid quantity format.");
                return;
            }

            _productService.UpdateProductStock(id, quantity);
            Console.WriteLine("Stock updated successfully.");
        }

        private static Product GetProductDetailsFromUser()
        {
            Console.Write("\nEnter product name: ");
            var name = Console.ReadLine();

            Console.Write("Enter product description (optional): ");
            var description = Console.ReadLine();

            Console.Write("Enter price: ");
            decimal price = 0;
            decimal.TryParse(Console.ReadLine(), out price);

            Console.Write("Enter stock quantity: ");
            int quantity = 0;
            int.TryParse(Console.ReadLine(), out quantity);

            return new Product
            {
                Name = name,
                Description = string.IsNullOrWhiteSpace(description) ? null : description,
                Price = price,
                StockQuantity = quantity
            };
        }

        private static void DisplayProducts(List<Product> products)
        {
            if (products.Count == 0)
            {
                Console.WriteLine("\nNo products found.");
                return;
            }

            Console.WriteLine("\nProducts List:");
            Console.WriteLine("-------------");
            foreach (var product in products)
            {
                Console.WriteLine("\n" + product.ToString());
            }
        }
    }
} 