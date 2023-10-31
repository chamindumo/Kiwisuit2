using Kiwisuit2.Data;
using Kiwisuit2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using DbContext = Kiwisuit2.Data.DbContext;

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

        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _dbContext.Products.Find(p => p.ProductId == productId).FirstOrDefaultAsync();
        }

        public async Task CreateProductAsync(Product product)
        {
            await _dbContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProductAsync(string id, Product updatedUser)
        {
            var filter = Builders<Product>.Filter.Eq(u => u.ProductId, id);
            var update = Builders<Product>.Update
                .Set(u => u.ProductId, updatedUser.ProductId)
                .Set(u => u.ExpirDate, updatedUser.ExpirDate)
                .Set(u => u.Name, updatedUser.Name)
                .Set(u => u.Price, updatedUser.Price)
                .Set(u => u.IsAvalable, updatedUser.IsAvalable)

                .Set(u => u.ExpirDate, updatedUser.ExpirDate)
                .Set(u => u.ImageData, updatedUser.ImageData)

                .Set(u => u.Description, updatedUser.Description);

            var updateResult = await _dbContext.Products.UpdateOneAsync(filter, update);

            if (updateResult.ModifiedCount > 0)
            {
                return true; // User was successfully updated
            }

            return false; // User with the given username was not fou
        }

        public async Task<bool> DeleteProductAsync(string productId)
        {
            var user = await _dbContext.Products.Find(p => p.ProductId == productId).FirstOrDefaultAsync();

            if (user != null)
            {
                return true; // User was successfully deleted
            }

            return false; // User with the given username was not found
        }
    }
}
