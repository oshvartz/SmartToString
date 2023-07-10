using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Text;

namespace SmartToStringSourceGenerator
{
    [Generator]
    public class SmartToStringGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Add the marker attribute to the compilation
            context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
                "AddToStringAttribute.g.cs",
                SourceText.From(SourceGenerationHelper.Attribute, Encoding.UTF8)));

            // TODO: implement the remainder of the source generator
        }
    }
}
