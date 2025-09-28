namespace Assignments.OOP.AutoService
{
    class Part
    {
        public Part(PartType partType, int price)
        {
            PartType = partType;
            Price = price;
        }

        public PartType PartType { get; private set; }
        public int Price { get; private set; }

        public void ShowInfo()
        {
            Console.WriteLine($"Part: {PartType}, Price: ${Price}");
        }
    }
}
