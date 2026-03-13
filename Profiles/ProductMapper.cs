using AutoMapper;
using ProductCQRS.Model;

namespace ProductCQRS.Profiles
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Product, ProductViewProfile>().ReverseMap();
        }
    }
}