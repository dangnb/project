using FluentValidation;

namespace MNG.Contract.Services.V1.Role.Validators;
public class CreateRoleValidator : AbstractValidator<Command.CreateRoleCommand>
{
    public CreateRoleValidator()
    {
        RuleFor(x => x.RoleCode).NotEmpty();
        RuleFor(x => x.Description);
    }
}

