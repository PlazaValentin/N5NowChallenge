using FluentValidation;
using N5NowChallenge.Application.Commands.UpdatePermission;

namespace N5NowChallenge.API.Validators;

public class UpdatePermissionCommandValidator : AbstractValidator<UpdatePermissionCommand>
{
    public UpdatePermissionCommandValidator()
    {
        RuleFor(c => c.PermissionId)
            .NotEmpty()
            .GreaterThan(0);

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