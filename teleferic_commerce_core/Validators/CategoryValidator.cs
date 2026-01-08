using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teleferic_commerce_core.DTO.Category;

namespace teleferic_commerce_core.Validators
{
    public class CategoryValidator : AbstractValidator<CreateCategoryDTO>
    {
        public CategoryValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MinimumLength(10).WithMessage("Category name must not exceed 100 characters.");
            RuleFor(c => c.Description)
                .MaximumLength(500).WithMessage("Category description must not exceed 500 characters.");
        }
    }
}
