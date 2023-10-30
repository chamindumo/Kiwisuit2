using Kiwisuit2.Data;
using Kiwisuit2.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Kiwisuit2.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbContext _dbContext;

        public ProductRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _dbContext.Products.Find(_ => true).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(string productId)
        {
            if (!IsValidObjectIdFormat(productId))
            {
                // Handle invalid format, for example, return a 400 Bad Request response.
                return null;
            }

            var filter = Builders<Product>.Filter.Eq(p => p.ProductId, productId);
            return await _dbContext.Products.Find(filter).FirstOrDefaultAsync();
        }


        public async Task CreateProductAsync(Product product)
        {
            await _dbContext.Products.InsertOneAsync(product);
        }

        public async Task UpdateProductAsync(string productId, Product updatedProduct)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.ProductId, productId);
            var update = Builders<Product>.Update
                .Set(p => p.Name, updatedProduct.Name)
                .Set(p => p.Price, updatedProduct.Price)
                .Set(p => p.Description, updatedProduct.Description)
                .Set(p => p.IsAvalable, updatedProduct.IsAvalable)
                .Set(p => p.ExpirDate, updatedProduct.ExpirDate)
                .Set(p => p.ImageData, updatedProduct.ImageData);

            await _dbContext.Products.UpdateOneAsync(filter, update);
        }

        public async Task DeleteProductAsync(string productId)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.ProductId, productId);
            await _dbContext.Products.DeleteOneAsync(filter);
        }


        private bool IsValidObjectIdFormat(string input)
        {
            if (string.IsNullOrEmpty(input) || input.Length != 24)
            {
                return false;
            }

            return System.Text.RegularExpressions.Regex.IsMatch(input, "^[0-9a-fA-F]+$");
        }

    }
}
