using eShopSolution.ViewModels.System.User;
using FluentValidation;


namespace eShopSolution.ViewModels.System.SystemValidator
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Bạn cần điền tên đăng nhập");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Bạn cần điền mật khẩu")
                .MinimumLength(6).WithMessage("Mật khẩu cần ít nhất 6 ký tự");
        }
    }
}
