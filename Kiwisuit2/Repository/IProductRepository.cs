using global::Kiwisuit2.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Kiwisuit2.Repository
{
    

    
        public interface IProductRepository
        {
            Task<IEnumerable<Product>> GetAllProductsAsync();
            Task<Product> GetProductByIdAsync(string id);
            Task CreateProductAsync(Product product);
            Task<bool> UpdateProductAsync(string id, Product updatedUser);
            Task<bool> DeleteProductAsync(string productId);
    }
    

}
