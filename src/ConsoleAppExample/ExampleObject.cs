using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppExample
{
    public class ExampleObject
    {

        [CodeGen.SmartStringGenerators.AddToString]
        public string StrProperty { get; set; }
    }
}
