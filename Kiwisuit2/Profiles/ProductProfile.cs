using Kiwisuit2.DTO;
using Kiwisuit2.Models;
using AutoMapper;


namespace Kiwisuit2.Profiles
{
    public class ProductProfile: Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDTO, Product>();
            CreateMap<Product, ProductDTO>();
        }

    }
}
