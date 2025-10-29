using System.Reflection;

namespace MNG.Application;
public static class AssemblyReference
{
    public static readonly Assembly assembly = typeof(AssemblyReference).Assembly;
}
