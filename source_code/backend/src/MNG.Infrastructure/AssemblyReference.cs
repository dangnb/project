using System.Reflection;

namespace MNG.Infrastructure;

public static class AssemblyReference
{
    public static readonly Assembly assembly = typeof(AssemblyReference).Assembly;
}
