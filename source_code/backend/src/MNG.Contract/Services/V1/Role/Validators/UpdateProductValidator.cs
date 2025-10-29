//using MNG.Contract.Services.V1.Product;
//using FluentValidation;

//namespace MNG.Contract.Services.V1.Product.Validators;
//public class UpdateProductValidator : AbstractValidator<Command.UpdateProductCommand>
//{
//    public UpdateProductValidator()
//    {
//        RuleFor(x => x.Id).NotEmpty();
//        RuleFor(x => x.Name).NotEmpty();
//        RuleFor(x => x.Price).GreaterThan(0);
//        RuleFor(x => x.Description).NotEmpty();
//    }
//}
