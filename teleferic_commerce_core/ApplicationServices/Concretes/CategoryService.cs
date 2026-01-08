using AutoMapper;
using FluentValidation;
using teleferic_commerce_core.ApplicationServices.Interfaces;
using teleferic_commerce_core.DTO.Category;
using teleferic_commerce_core.Validators;
using teleferic_commerce_infrastructure.Models;
using teleferic_commerce_infrastructure.UoW;
using teleferic_core_domain.Entities;

namespace teleferic_commerce_core.ApplicationServices.Concretes
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;
        private readonly IValidator<CreateCategoryDTO> _categoryValidator;
        public CategoryService(IUnitOfWork unitOfWork,IMapper mapper,IValidator<CreateCategoryDTO> categoryValidator)
        {
            _categoryValidator = categoryValidator;
            this.mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseModel<CategoryDTO>> CreateCategory(CreateCategoryDTO categoryDTO)
        {

           var result = await _categoryValidator.ValidateAsync(categoryDTO);

            if (!result.IsValid)
            {
                List<string> messages = new List<string>();
                result.Errors.ForEach(x => messages.Add(x.ErrorMessage.ToString()));
            }
            var category = mapper.Map<Category>(categoryDTO);
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveAsync();
            var categoryData = mapper.Map<CategoryDTO>(category);
            return new ResponseModel<CategoryDTO>
            {
                IsSuccess = true,
                Data = categoryData,
                Message = "Category created successfully."
            };
        }

        public async Task<ResponseModel<bool>> DeleteCategory(Guid Id)
        {
            var category = _unitOfWork.Categories.GetByIdAsync(Id);
            if (category == null)
            {
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = "Category not found."
                };
            }
            await _unitOfWork.Categories.SoftDeleteAsync(Id);
            await _unitOfWork.SaveAsync();
            return new ResponseModel<bool>
            {
                IsSuccess = true,
                Data = true,
                Message = "Category deleted successfully."
            };
        }


        public async Task<ResponseModel<IEnumerable<CategoryDTO>>> GetCategories()
        {
            IEnumerable<Category> categories = await _unitOfWork.Categories.GetAllAsync();
            var categoryDTOs = mapper.Map<IEnumerable<CategoryDTO>>(categories);
            return new ResponseModel<IEnumerable<CategoryDTO>>
            {
                IsSuccess = true,
                Data = categoryDTOs,
                Message = "Categories retrieved successfully."
            };
        }

        public async Task<ResponseModel<CategoryDTO>> GetCategory(Guid Id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(Id);
            if (category == null)
            {
                return new ResponseModel<CategoryDTO>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Category not found."
                };
            }
            var categoryDTO = mapper.Map<CategoryDTO>(category);
            return new ResponseModel<CategoryDTO>
            {
                IsSuccess = true,
                Data = categoryDTO,
                Message = "Category retrieved successfully."
            };
        }

        public async Task<ResponseModel<CategoryDTO>> UpdateCategory(Guid Id, UpdateCategoryDTO categoryDTO)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(Id);
            if (category == null)
            {
                return new ResponseModel<CategoryDTO>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Category not found."
                };
            }
            mapper.Map(categoryDTO, category);
            await _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveAsync();
            var updatedCategoryDTO = mapper.Map<CategoryDTO>(category);
            return new ResponseModel<CategoryDTO>
            {
                IsSuccess = true,
                Data = updatedCategoryDTO,
                Message = "Category updated successfully."
            };
        }
    }
}
