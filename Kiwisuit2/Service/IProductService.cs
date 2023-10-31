using Kiwisuit2.DTO;
using Kiwisuit2.Models;

namespace Kiwisuit2.Service
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllAsync();
        Task<ProductDTO> GetByIdAsync(string id);
        Task CreateAsync(ProductDTO productDTO);
        Task UpdateAsync(string id, ProductDTO productDTO);
        Task DeleteAsync(string id);

    }
}
