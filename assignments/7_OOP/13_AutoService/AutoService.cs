namespace Assignments.OOP.AutoService
{
    class AutoService
    {
        private int _balance;
        private Queue<Car> _carQueue = new Queue<Car>();
        private Inventory _inventory = new Inventory();

        public AutoService(int initialBalance, List<Car> carQueue, Inventory inventory)
        {
            _balance = initialBalance;
            _carQueue = new Queue<Car>(carQueue);
            _inventory = inventory;
        }

        public void ShowCarsInQueue()
        {
            Console.WriteLine("Cars in queue:");

            foreach (Car car in _carQueue)
            {
                Console.WriteLine($"- {car.Model}");
            }
        }
    }
}
