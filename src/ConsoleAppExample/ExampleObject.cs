namespace ConsoleAppExample
{
    public class ExampleObject
    {

        [CodeGen.SmartStringGenerators.AddToString]
        public string StrProperty { get; set; }

        [CodeGen.SmartStringGenerators.AddToString]
        public string StrProperty2 { get; set; }

        [CodeGen.SmartStringGenerators.AddToString]
        public int IntProperty { get; set; }

        public int IntProperty2 { get; set; }
    }
}
