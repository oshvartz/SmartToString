using CodeGen.SmartStringGenerators;

namespace ConsoleAppExample
{
    public class Car
    {
        [AddToString]
        public string LicencePlateNumber { get; set; }
        public string OwnerId { get; set; }
        [AddToString]
        public CarBrand Brand { get; set; }
    }

    public enum CarBrand
    {
        Honda,
        VW,
        Fiat
    }
}
