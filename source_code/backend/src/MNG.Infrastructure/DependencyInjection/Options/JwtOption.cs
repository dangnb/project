namespace MNG.Infrastructure.DependencyInjection.Options;
public class JwtOption
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public int ExpiredMin { get; set; }
}
