using AutoMapper;
using teleferic_commerce_core.DTO.Category;
using teleferic_commerce_core.DTO.Product;
using teleferic_core_domain.Entities;

namespace teleferic_commerce_core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Category
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, UpdateCategoryDTO>().ReverseMap();
            CreateMap<Category, CreateCategoryDTO>().ReverseMap();
            #endregion

            #region Product
            CreateMap<Product,ProductDTO>()
                .ForMember(dest => dest.CategoryName,opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ProductImages)).ReverseMap();

            CreateMap<Product, CreateProductDTO>().ReverseMap();
            CreateMap<Product, UpdateProductDTO>().ReverseMap();
            #endregion

            #region ProductImage
            CreateMap<ProductImage, ProductImageDto>().ReverseMap();
            CreateMap<ProductImage, AddProductImageDto>().ReverseMap();
            #endregion

            #region Address
            CreateMap<Address, DTO.Address.AddressDTO>().ReverseMap();
            CreateMap<Address, DTO.Address.CreateAddressDTO>().ReverseMap();
            CreateMap<Address, DTO.Address.UpdateAddressDTO>().ReverseMap();
            #endregion

            #region Basket
            CreateMap<Basket, DTO.Basket.BasketDTO>().ReverseMap();
            CreateMap<BasketItem, DTO.Basket.BasketItemDto>().ReverseMap();

            #endregion

            #region Order
            CreateMap<Order?, DTO.Order.OrderDto>().ReverseMap();
            CreateMap<OrderItem, DTO.Order.OrderItemDto>().ReverseMap();
            #endregion

        }
    }
}
