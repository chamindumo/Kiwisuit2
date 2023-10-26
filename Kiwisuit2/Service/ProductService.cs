using AutoMapper;
using Kiwisuit2.DTO;
using Kiwisuit2.Models;
using Kiwisuit2.Repository;

namespace Kiwisuit2.Service
{
    public class ProductService:IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO> GetByIdAsync(string id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task CreateAsync(ProductDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            await _productRepository.CreateProductAsync(product);
        }

        public async Task UpdateAsync(string id, ProductDTO updatedProductDto)
        {
            var updatedProduct = _mapper.Map<Product>(updatedProductDto);
            await _productRepository.UpdateProductAsync(id, updatedProduct);
        }

        public async Task DeleteAsync(string id)
        {
            await _productRepository.DeleteProductAsync(id);
        }
    }
}

