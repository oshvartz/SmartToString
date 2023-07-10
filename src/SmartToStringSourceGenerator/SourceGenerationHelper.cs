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


        public const string SmartToStringExtensionPrefix = 
@"using System.Text;

namespace CodeGen.SmartStringGenerators
{
    internal static class SmartToStringExtension
    {";

        public static string GenerateSmartToStringExtension(TypesWithProperties typesWithProperties)
        {
            var sb = new StringBuilder();
            sb.Append(SmartToStringExtensionPrefix);
            foreach (var type in typesWithProperties.TypeWithProperties)
            {
                sb.AppendLine(GenerateToStringSmartMethod(type));
            }

            sb.Append(@"
    }
}");
            return sb.ToString();
        }

        private static string GenerateToStringSmartMethod(TypeWithProperties typeWithProperties)
        {
            var sb = new StringBuilder();
            var classParamName = ToPascalCase(typeWithProperties.ClassName);

            sb.Append(@$"
        public static string ToStringSmart(this {typeWithProperties.Namespace}.{typeWithProperties.ClassName} {classParamName})
        {{
            var sb = new StringBuilder();");
            sb.AppendLine(string.Empty);

            foreach (var prop in typeWithProperties.ToStringProperties)
            {
                sb.AppendLine(@$"            sb.Append($""{{nameof({classParamName}.{prop.PropertyName})}}:{{{classParamName}.{prop.PropertyName}}}|"");");
            }
            sb.Append(@$"
            return sb.ToString();
        }}");


            return sb.ToString(); ;
        }

        private static string ToPascalCase(string className)
        {
            return className[0].ToString().ToLower() + className.Substring(1);
        }
    }
}
