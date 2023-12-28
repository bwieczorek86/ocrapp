using FluentValidation;
using Microsoft.Extensions.Localization;

namespace OcrPlugin.App.BlazorClient.Server.Components.Auth
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        private readonly IStringLocalizer<OcrPlugin.App.Language.Resources.App> _stringLocalizer;

        public LoginRequestValidator(IStringLocalizer<OcrPlugin.App.Language.Resources.App> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;

            RuleFor(x => x.UserName).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(_stringLocalizer["Validation_Field_Not_Empty"])
                .NotNull().WithMessage(_stringLocalizer["Validation_Field_Not_Empty"])
                .MaximumLength(50);

            RuleFor(x => x.Password).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(_stringLocalizer["Validation_Field_Not_Empty"])
                .NotNull().WithMessage(_stringLocalizer["Validation_Field_Not_Empty"])
                .MaximumLength(32);
        }
    }
}