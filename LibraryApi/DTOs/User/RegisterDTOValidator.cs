using FluentValidation;
using LibraryApi.Data;
using System.Text.RegularExpressions;

namespace LibraryApi.DTOs.User
{
    public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
    {
        public bool ValidatePassword(string pw)
        {
            var lowercase = new Regex("[a-z]+");
            var uppercase = new Regex("[A-Z]+");
            var digit = new Regex("(\\d)+");
            var symbol = new Regex("(\\W)+");

            return lowercase.IsMatch(pw) && uppercase.IsMatch(pw) && digit.IsMatch(pw) && symbol.IsMatch(pw);
        }
        public RegisterDTOValidator(ApplicationDbContext dbContext)
        {
            RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress();

            RuleFor(x => x.Password)
                .MinimumLength(8)
                .Must(HasValidPassword);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .Equal(e => e.Password)
                .WithMessage("Password is not the same");

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Users.Any(u => u.Email == value);
                    if (emailInUse)
                    {
                        context.AddFailure("Email", "That email is taken");
                    }
                });
            RuleFor(x => x.Role)
                .NotEmpty()
                .IsInEnum();
        }

        private bool HasValidPassword(string pw)
        {
            var lowercase = new Regex("[a-z]+");
            var uppercase = new Regex("[A-Z]+");
            var digit = new Regex("(\\d)+");
            var symbol = new Regex("(\\W)+");

            return lowercase.IsMatch(pw) && uppercase.IsMatch(pw) && digit.IsMatch(pw) && symbol.IsMatch(pw);
        }
    }

}
