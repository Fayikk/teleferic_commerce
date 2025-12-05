using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teleferic_commerce_core.DTO.Category;
using teleferic_commerce_infrastructure.Models;

namespace teleferic_commerce_core.ApplicationServices.Interfaces
{
    public interface ICategoryService
    {
        Task<ResponseModel<IEnumerable<CategoryDTO>>> GetCategories();
        Task<ResponseModel<CategoryDTO>> GetCategory(Guid Id);

        Task<ResponseModel<CategoryDTO>> CreateCategory(CreateCategoryDTO categoryDTO);
        Task<ResponseModel<CategoryDTO>> UpdateCategory(Guid Id, UpdateCategoryDTO categoryDTO);
        Task<ResponseModel<bool>> DeleteCategory(Guid Id);
    }
}
