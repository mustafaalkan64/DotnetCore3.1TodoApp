using FluentValidation;
using Todo_App.Business.Models;

namespace Todo_App.Business.Validations
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(u => u.FirstName)
                .NotEmpty()
                .WithMessage("Please Enter FirstName")
                .MaximumLength(50)
                .WithMessage("FirstName Maximum Length Should Be 50");
            RuleFor(u => u.LastName)
                .NotEmpty()
                .WithMessage("Please Enter LastName")
                .MaximumLength(50)
                .WithMessage("LastName Maximum Length Should Be 50");
            RuleFor(u => u.Password)
                .NotEmpty()
                .WithMessage("Please Enter Password")
                .MaximumLength(50)
                .WithMessage("Password Maximum Length Should Be 50")
                .MinimumLength(8)
                .WithMessage("Password Minimum Length Should Be 8");
            RuleFor(u => u.UserName)
                 .NotEmpty()
                 .WithMessage("Please Enter UserName")
                 .MaximumLength(100)
                 .WithMessage("LastName Maximum Length Should Be 100");
        }
    }

    public class UserLoginValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginValidator()
        {

            RuleFor(u => u.Email)
                .NotEmpty()
                .WithMessage("Please Enter Email")
                .MaximumLength(150)
                .WithMessage("Email Maximum Length Should Be 150")
                .EmailAddress()
                .WithMessage("Email Format Is Invalid");
            RuleFor(u => u.Password)
                .NotEmpty()
                .WithMessage("Please Enter Password");
        }
    }
}
