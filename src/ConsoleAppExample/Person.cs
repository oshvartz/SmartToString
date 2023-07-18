using CodeGen.SmartStringGenerators;
namespace ConsoleAppExample
{
    public class Person
    {
        [AddToString]
        public string FirstName { get; set; }

        [AddToString]
        public string LastName { get; set; }
   
        public int SomeSecret { get; set; }

        [AddToString]
        public int Id { get; set; }
    }
}
