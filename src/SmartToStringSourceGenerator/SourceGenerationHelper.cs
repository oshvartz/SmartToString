using System;
using System.Collections.Generic;
using System.Text;

namespace SmartToStringSourceGenerator
{
    public static class SourceGenerationHelper
    {
        public const string Attribute = @"
namespace CodeGen.SmartStringGenerators
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    internal class AddToStringAttribute : System.Attribute
    {
    }
}";
    }
}
