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

var car = new Car
{
    OwnerId = "Homer",
    Brand = CarBrand.Honda,
    LicencePlateNumber = "007"
};

Console.WriteLine(car.ToStringSmart());