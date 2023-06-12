using System;
using JetBrains.Annotations;

namespace DependencyInjection
{
    [MeansImplicitUse]
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class InjectAttribute : Attribute
    {
    }
}