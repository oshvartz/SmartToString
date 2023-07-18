using CodeGen.SmartStringGenerators;
using ConsoleAppExample;

var person = new Person
{
    FirstName = "Homer",
    LastName = "Simpson",
    SomeSecret = 101,
    Id = 1234
};

Console.WriteLine(person.ToStringSmart());