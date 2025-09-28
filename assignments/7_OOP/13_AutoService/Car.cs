namespace Assignments.OOP.AutoService
{
    class Car
    {
        private List<Part> _brokenParts = new List<Part>();

        public Car()
        {

        }

        public string Model { get; private set; }
    }
}
