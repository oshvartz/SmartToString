using CodeGen.SmartStringGenerators;
using ConsoleAppExample;

var exampleObject = new ExampleObject
{
    StrProperty = "Prop1",
    StrProperty2 = "Prop2",
    IntProperty = 1,
    IntProperty2 = 2
};

Console.WriteLine(exampleObject.ToStringSmart());