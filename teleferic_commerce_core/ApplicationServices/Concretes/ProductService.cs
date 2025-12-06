using AutoMapper;
using Microsoft.EntityFrameworkCore;
using teleferic_commerce_core.ApplicationServices.Interfaces;
using teleferic_commerce_core.DTO.Product;
using teleferic_commerce_infrastructure.Models;
using teleferic_commerce_infrastructure.UoW;
using teleferic_core_domain.Entities;

namespace teleferic_commerce_core.ApplicationServices.Concretes
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        public ProductService(IUnitOfWork uow,IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<ResponseModel<ProductImageDto>> AddProductImage(Guid Id, AddProductImageDto addImageDTO)
        {
            var product = await uow.Products.GetAllAsyncWithInclude(
                predicate:x=>x.Id == Id,
                includeExpression: p => p.Include(pr => pr.ProductImages)
                );
            if (product is null)
            {
                return new ResponseModel<ProductImageDto>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "Product not found"
                };
            }

            if (addImageDTO.Image == null || addImageDTO.Image.Length == 0)
            {
                return new ResponseModel<ProductImageDto>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "Invalid image file"
                };
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(addImageDTO.Image.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                return new ResponseModel<ProductImageDto>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "Unsupported image format"
                };
            }

            var fileName = $"{Guid.NewGuid()}{extension}";
            var imagePath = Path.Combine("wwwroot", "images", fileName);
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            var filePath = Path.Combine(imagePath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await addImageDTO.Image.CopyToAsync(stream);
            }

            if (addImageDTO.IsMain)
            {
                var currentMainImage = product.FirstOrDefault().ProductImages.FirstOrDefault(img => img.IsMain);
                if (currentMainImage != null)
                {
                    currentMainImage.IsMain = false;
                    await uow.ProductImages.Update(currentMainImage);
                    await uow.SaveAsync();

                }

            }

            var productImage = new ProductImage
            {
                ProductId = Id,
                ImageUrl = $"/images/{fileName}/{fileName}",
                IsMain = addImageDTO.IsMain
            };

            await uow.ProductImages.AddAsync(productImage);
            await uow.SaveAsync();
            var productImageDto = mapper.Map<ProductImageDto>(productImage);
            return new ResponseModel<ProductImageDto>
            {
                Data = productImageDto,
                IsSuccess = true,
                Message = "Image added successfully"
            };
        }
        public async Task<ResponseModel<bool>> DeleteProductImage(Guid productId, Guid imageId)
        {
            var image = await uow.ProductImages.FindAsync(x => x.Id == imageId);
            if (image == null)
            {
                return new ResponseModel<bool>
                {
                    Data = false,
                    IsSuccess = false,
                    Message = "Image not found"
                };
            }
            if (!string.IsNullOrEmpty(image.FirstOrDefault().ImageUrl))
            {
                var fileName = Path.GetFileName(image.FirstOrDefault().ImageUrl);
                var filePath = Path.Combine("wwwroot", "images", fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            await uow.ProductImages.SoftDeleteAsync(imageId);
            await uow.SaveAsync();
            return new ResponseModel<bool>
            {
                Data = true,
                IsSuccess = true,
                Message = "Image deleted successfully"
            };

        }
        public async Task<ResponseModel<ProductDTO>> CreateProduct(CreateProductDTO createProductDTO)
        {
            var objMap = mapper.Map<Product>(createProductDTO);
            await uow.Products.AddAsync(objMap);
            await uow.SaveAsync();
            var productDTO = mapper.Map<ProductDTO>(objMap);
            return new ResponseModel<ProductDTO>
            {
                Data = productDTO,
                IsSuccess = true,
                Message = "Product created successfully"
            };
        }

        public async Task<ResponseModel<bool>> DeleteProduct(Guid Id)
        {
            var product = await uow.Products.GetByIdAsync(Id);
            if (product == null)
            {
                return new ResponseModel<bool>
                {
                    Data = false,
                    IsSuccess = false,
                    Message = "Product not found"
                };
            }

            await uow.Products.SoftDeleteAsync(Id);
            await uow.SaveAsync();
            return new ResponseModel<bool>
            {
                Data = true,
                IsSuccess = true,
                Message = "Product deleted successfully"
            };
        }
        public async Task<ResponseModel<IEnumerable<ProductDTO>>> GetProducts(Guid categoryId)
        {

            if (categoryId.ToString().StartsWith("000"))
            {
                var products = await uow.Products.GetAllAsyncWithInclude(
                 null,
                 includeExpression: p => p.Include(pr => pr.ProductImages).Include(x => x.Category)
             );
                var productsDTO = mapper.Map<IEnumerable<ProductDTO>>(products);
                var response = new ResponseModel<IEnumerable<ProductDTO>>
                {
                    Data = productsDTO,
                    IsSuccess = true,
                    Message = "Products retrieved successfully"
                };
                return response;
            }
            else
            {
                var products = await uow.Products.GetAllAsyncWithInclude(
               predicate: p => p.CategoryId == categoryId,
               includeExpression: p => p.Include(pr => pr.ProductImages)
           );
                var productsDTO = mapper.Map<IEnumerable<ProductDTO>>(products);
                var response = new ResponseModel<IEnumerable<ProductDTO>>
                {
                    Data = productsDTO,
                    IsSuccess = true,
                    Message = "Products retrieved successfully"
                };
                return response;
            }

           
        }

        public async Task<ResponseModel<ProductDTO>> GetProductById(Guid Id)
        {
            var products = await uow.Products.GetAllAsyncWithInclude(
                predicate: x => x.Id == Id,
                includeExpression: p => p.Include(pr => pr.ProductImages).Include(x => x.Category)
                );
            var product = products.FirstOrDefault();
            var productDTO = mapper.Map<ProductDTO>(product);   
            return new ResponseModel<ProductDTO>
            {
                Data = productDTO,
                IsSuccess = true,
                Message = "Product retrieved successfully"
            };
        }

        public async Task<ResponseModel<ProductDTO>> UpdateProduct(Guid Id, UpdateProductDTO dto)
        {
            var product = await uow.Products.GetByIdAsync(Id);
            if (product == null)
            {
                return new ResponseModel<ProductDTO>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "Product not found"
                };
            }
            mapper.Map(dto, product);
            await uow.Products.Update(product);
            await uow.SaveAsync();
            var productDTO = mapper.Map<ProductDTO>(product);
            return new ResponseModel<ProductDTO>
            {
                Data = productDTO,
                IsSuccess = true,
                Message = "Product updated successfully"
            };
        }
    }
}
