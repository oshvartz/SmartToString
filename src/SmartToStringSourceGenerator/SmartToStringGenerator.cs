using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;

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

            #region Dymamic stuff Starts Here

            IncrementalValuesProvider<PropertyDeclarationSyntax> propertyDeclarations = context.SyntaxProvider
           .CreateSyntaxProvider(
               predicate: static (s, _) => IsSyntaxTargetForGeneration(s), // select enums with attributes
               transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx)) // sect the method with the [AddToStringAttribute] attribute
           .Where(static m => m is not null)!; // filter out attributed enums that we don't care about

            // Combine the selected methods with the `Compilation`
            IncrementalValueProvider<(Compilation, ImmutableArray<PropertyDeclarationSyntax>)> compilationAndProperties
                = context.CompilationProvider.Combine(propertyDeclarations.Collect());

            // Generate the source using the compilation and properties
            context.RegisterSourceOutput(compilationAndProperties,
                static (spc, source) => Execute(source.Item1, source.Item2, spc));

            #endregion
        }


        private static PropertyDeclarationSyntax GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            // we know the node is a PropertyDeclarationSyntax thanks to IsSyntaxTargetForGeneration
            var propertyDeclarationSyntax = (PropertyDeclarationSyntax)context.Node;

            // loop through all the attributes on the property
            foreach (AttributeListSyntax attributeListSyntax in propertyDeclarationSyntax.AttributeLists)
            {
                foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
                {
                    if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                    {
                        // weird, we couldn't get the symbol, ignore it
                        continue;
                    }

                    INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                    string fullName = attributeContainingTypeSymbol.ToDisplayString();


                    if (fullName == "CodeGen.SmartStringGenerators.AddToStringAttribute")
                    {
                        // return the property
                        return propertyDeclarationSyntax;
                    }
                }
            }

            // we didn't find the attribute we were looking for
            return null;
        }

        private static bool IsSyntaxTargetForGeneration(SyntaxNode node)
        {
            return node is PropertyDeclarationSyntax propertyDeclaration && propertyDeclaration.AttributeLists.Count > 0;
        }

        private static void Execute(Compilation compilation, ImmutableArray<PropertyDeclarationSyntax> properties, SourceProductionContext context)
        {
            if (properties.IsDefaultOrEmpty)
            {
                // nothing to do yet
                return;
            }

            // I'm not sure if this is actually necessary, but `[LoggerMessage]` does it, so seems like a good idea!
            IEnumerable<PropertyDeclarationSyntax> distinctProperties = properties.Distinct();

            var typesWithProperties = GetTypesWithProperies(compilation, distinctProperties, context.CancellationToken);

            string result = SourceGenerationHelper.GenerateSmartToStringExtension(typesWithProperties);
            context.AddSource($"SmartToStringExtension.g.cs", SourceText.From(result, Encoding.UTF8));
        }

        private static TypesWithProperties GetTypesWithProperies(Compilation compilation, IEnumerable<PropertyDeclarationSyntax> distinctProperties, CancellationToken cancellationToken)
        {
            var classToProperties = new Dictionary<string, List<PropertyDeclarationSyntax>>();

            foreach (var property in distinctProperties)
            {
                var classProp = GetClassProp(property);
                var fullClassName = $"{classProp.nameSpace}.{classProp.className}";
                if (!classToProperties.ContainsKey(fullClassName))
                {
                    classToProperties.Add(fullClassName, new List<PropertyDeclarationSyntax>());
                }
                classToProperties[fullClassName].Add(property);
            }

            return new TypesWithProperties
            {
                TypeWithProperties = classToProperties.Select(classAndProprties =>
                {

                    var classProp = GetClassProp(classAndProprties.Value.First());
                    return new TypeWithProperties
                    {
                        ClassName = classProp.className,
                        Namespace = classProp.nameSpace,
                        ToStringProperties = classAndProprties.Value.Select(p => new ToStringProperty
                        {
                            PropertyName = p.Identifier.ToString()
                        })

                    };
                })
            };
        }

        private static (string className, string nameSpace) GetClassProp(PropertyDeclarationSyntax property)
        {
            var classDeclarationSyntax = (ClassDeclarationSyntax)property.Parent;
            var className = classDeclarationSyntax.Identifier.ToString();
            var ns = (classDeclarationSyntax.Parent as NamespaceDeclarationSyntax).Name.ToString();
            return (className, ns);
        }   
    }
}
