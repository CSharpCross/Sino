using FluentValidation;
using OrvilleX.Validation;

namespace OrvilleXIntegrationTest.Fake
{
    public class InjectsExplicitChildValidator : BaseRequestValidator<ParentModel>
    {
        public InjectsExplicitChildValidator()
        {
            RuleFor(x => x.Child).InjectValidator();
        }
    }

    public class InjectedChildValidator: BaseRequestValidator<ChildModel>
    {
        public InjectedChildValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("NotNullInjected");
        }
    }

    public class InjectsExplicitChildValidatorCollection : AbstractValidator<ParentModel6>
    {
        public InjectsExplicitChildValidatorCollection()
        {
            RuleForEach(x => x.Children).InjectValidator();
        }
    }
}
