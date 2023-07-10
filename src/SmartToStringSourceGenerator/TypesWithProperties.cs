using System;
using System.Collections.Generic;
using System.Text;

namespace SmartToStringSourceGenerator
{
    public class TypesWithProperties
    {
        public IEnumerable<TypeWithProperties> TypeWithProperties { get; set; }
    }

    public class TypeWithProperties
    {
        public string ClassName { get; set; }

        public string Namespace { get; set; }

        public IEnumerable<ToStringProperty> ToStringProperties { get; set; }
    }

    public class ToStringProperty
    {
        public string PropertyName { get; set; }
    }
}
