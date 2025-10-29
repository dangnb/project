namespace MNG.Persistence.DependencyInjection.Options;
public class PasswordValidatorOptions
{
    public int RequiredMinLength { get; set; }
    public int RequiredMaxLength { get; set; } = int.MaxValue;
    public int RequireNonAlphanumeric { get; set; }
    public int RequireDigit { get; set; }
    public int RequireLowercase { get; set; }
    public int RequireUppercase { get; set; }
    public int RequiredDigitLength { get; set; }
}
