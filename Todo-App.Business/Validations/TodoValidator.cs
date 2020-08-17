using FluentValidation;
using Todo_App.Business.Models;

namespace Todo_App.Business.Validations
{
    public class TodoValidator : AbstractValidator<TodoDto>
    {
        public TodoValidator()
        {
            RuleFor(u => u.Name)
                .NotEmpty()
                .WithMessage("Please Enter Todo Name")
                .MaximumLength(150)
                .WithMessage("Name Field Maximum Length Should Be 150");
            RuleFor(u => u.Desc)
                .NotEmpty()
                .WithMessage("Please Enter Description")
                .MaximumLength(500)
                .WithMessage("Description Field Maximum Length Should Be 500");
        }
    }
}
