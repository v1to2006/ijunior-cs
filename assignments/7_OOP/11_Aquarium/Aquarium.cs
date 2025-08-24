namespace OOP.Assignments.Aquarium
{
    class Aquarium
    {
        private List<Fish> _fishes = new List<Fish>();
        private int _capacity;
        private int _cycle;

        public Aquarium()
        {
            _capacity = 5;
            _cycle = 0;
        }

        public void Run()
        {
            const string CommandAdd = "ADD";
            const string CommandRemove = "REMOVE";
            const string CommandExit = "EXIT";

            bool isContinue = true;

            while (isContinue)
            {
                Console.Clear();
                ShowInfo();
                Console.WriteLine();

                Console.WriteLine($"{CommandAdd}\tAdd fish to aquarium");
                Console.WriteLine($"{CommandRemove}\tRemove fish from aquarium");
                Console.WriteLine($"{CommandExit}\tExit the program");
                Console.WriteLine($"\nOr press any key to go one cycle\n");
                Console.Write("> ");
                string userInput = Console.ReadLine().ToUpper();
                Console.Clear();

                switch (userInput)
                {
                    case CommandAdd:
                        AddFish();
                        break;
                    case CommandRemove:
                        RemoveFish();
                        break;
                    case CommandExit:
                        isContinue = false;
                        continue;
                }

                LiveCycle();
            }
        }

        private void AddFish()
        {
            if (_fishes.Count >= _capacity)
            {
                Console.WriteLine("Aquarium is full");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter fish name: ");
            string fishName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(fishName))
            {
                Console.WriteLine("Fish name cannot be empty");
            }
            else
            {
                _fishes.Add(new Fish(fishName.Trim()));

                Console.WriteLine($"Fish {fishName} has been added to aquarium");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        private void RemoveFish()
        {
            if (_fishes.Count == 0)
            {
                Console.WriteLine("No fishes in aquarium");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            ShowAllFishes();
            Console.Write("Enter fish index to remove: ");
            int.TryParse(Console.ReadLine(), out int input);

            int fishIndexToRemove = input - 1;

            if (fishIndexToRemove >= 0 && fishIndexToRemove < _fishes.Count)
            {
                Fish fishToRemove = _fishes[fishIndexToRemove];
                _fishes.RemoveAt(fishIndexToRemove);
                Console.WriteLine($"Fish {fishToRemove.Name} has been removed from aquarium");
            }
            else
            {
                Console.WriteLine("Invalid fish index");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        private void LiveCycle()
        {
            _cycle++;

            for (int i = _fishes.Count - 1; i >= 0; i--)
            {
                Fish currentFish = _fishes[i];
                currentFish.Grow();

                if (currentFish.Age >= currentFish.DeadAge)
                {
                    Console.WriteLine($"Fish {currentFish.Name} has died of old age ({currentFish.DeadAge} y.o)");
                    Console.ReadKey();

                    _fishes.Remove(currentFish);
                }
            }
        }

        private void ShowInfo()
        {
            Console.WriteLine($"Current cycle: {_cycle}\n");
            Console.WriteLine($"Aquarium's capacity: {_capacity}");

            ShowAllFishes();
        }

        private void ShowAllFishes()
        {
            Console.WriteLine("Fishes in aquarium:");

            if (_fishes.Count == 0)
            {
                Console.WriteLine("No fishes");
            }
            else
            {
                for (int i = 0; i < _fishes.Count; i++)
                {
                    Fish fish = _fishes[i];
                    Console.WriteLine($"{i + 1}. {fish.GetInfo()}");
                }
            }
        }
    }

    class Fish
    {
        public Fish(string name)
        {
            Name = name;
            Age = 0;
            DeadAge = GenerateDeadAge();
        }

        public string Name { get; private set; }
        public int Age { get; private set; }
        public int DeadAge { get; private set; }

        public void Grow()
        {
            Age++;
        }

        public string GetInfo()
        {
            return $"{Name} ({Age} y.o)";
        }

        private int GenerateDeadAge()
        {
            int minAge = 3;
            int maxAge = 10;

            return Utils.GetRandomNumber(minAge, maxAge + 1);
        }
    }

    class Utils
    {
        private static Random s_random = new Random();

        public static int GetRandomNumber(int minNumber, int maxNumber)
        {
            return s_random.Next(minNumber, maxNumber);
        }
    }
}
