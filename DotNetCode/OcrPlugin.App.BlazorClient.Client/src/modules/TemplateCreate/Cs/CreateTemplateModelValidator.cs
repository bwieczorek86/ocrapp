using FluentValidation;
using Microsoft.Extensions.Localization;

namespace OcrPlugin.App.BlazorClient.Client.modules.TemplateCreate.Cs
{
    public class CreateTemplateModelValidator : AbstractValidator<CreateTemplateModel>
    {
        private readonly IStringLocalizer<OcrPlugin.App.Language.Resources.App> _stringLocalizer;

        public CreateTemplateModelValidator(IStringLocalizer<OcrPlugin.App.Language.Resources.App> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;

            RuleFor(x => x.Name).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(_stringLocalizer["CreateTemplateModelValidator_Name_NotEmpty"])
                .NotNull().WithMessage(_stringLocalizer["CreateTemplateModelValidator_Name_NotEmpty"]);

            RuleFor(x => x.TitleTemplateMappings).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(_stringLocalizer["CreateTemplateModelValidator_Title_NotEmpty"])
                .NotNull().WithMessage(_stringLocalizer["CreateTemplateModelValidator_Title_NotEmpty"])
                .Custom(MustHaveValidTitle);
        }

        private void MustHaveValidTitle(
             ICollection<TitleTemplateMappingsDto> titleTemplateMappings,
             ValidationContext<CreateTemplateModel> context)
        {
            foreach (var title in titleTemplateMappings)
            {
                if (string.IsNullOrWhiteSpace(title.Title))
                {
                    context.AddFailure(_stringLocalizer["CreateTemplateModelValidator_Title_NotEmpty"]);
                    return;
                }
            }
        }
    }
}
