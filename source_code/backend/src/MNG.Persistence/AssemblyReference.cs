using System.Reflection;

namespace MNG.Persistence;

public static class AssemblyReference
{
    public static readonly Assembly assembly = typeof(AssemblyReference).Assembly;
}
