using FluentValidation;
using N5NowChallenge.Application.Commands.CreatePermission;

namespace N5NowChallenge.API.Validators;

public class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
{
    public CreatePermissionCommandValidator()
    {
        RuleFor(c => c.FirstNameEmployee)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(c => c.LastNameEmployee)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(c => c.PermissionTypeId)
            .NotEmpty()
            .GreaterThan(0)
            .LessThan(10);

        RuleFor(c => c.DatePermission)
            .NotEmpty();
    }
}
