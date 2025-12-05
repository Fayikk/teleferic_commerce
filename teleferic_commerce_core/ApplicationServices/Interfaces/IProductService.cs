using teleferic_commerce_core.DTO.Product;
using teleferic_commerce_infrastructure.Models;

namespace teleferic_commerce_core.ApplicationServices.Interfaces
{
    public interface IProductService
    {
        Task<ResponseModel<IEnumerable<ProductDTO>>> GetProducts(Guid categoryId);
        Task<ResponseModel<ProductDTO>> GetProductById(Guid Id);

        Task<ResponseModel<ProductDTO>> CreateProduct(CreateProductDTO createProductDTO);
        Task<ResponseModel<ProductDTO>> UpdateProduct(Guid Id, UpdateProductDTO dto);
        Task<ResponseModel<bool>> DeleteProduct(Guid Id);
        Task<ResponseModel<ProductImageDto>> AddProductImage(Guid Id, AddProductImageDto addImageDTO);
        Task<ResponseModel<bool>> DeleteProductImage(Guid productId, Guid imageId);

    }
}
