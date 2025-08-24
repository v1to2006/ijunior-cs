namespace OOP.Assignments.Aquarium
{
    class Aquarist
    {
        private Aquarium _aquarium = new Aquarium();

        public void Run()
        {
            const string CommandAdd = "ADD";
            const string CommandRemove = "REMOVE";
            const string CommandExit = "EXIT";

            bool isContinue = true;

            while (isContinue)
            {
                Console.Clear();
                _aquarium.ShowInfo();
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
                        HandleAddFish();
                        break;
                    case CommandRemove:
                        HandleRemoveFish();
                        break;
                    case CommandExit:
                        isContinue = false;
                        continue;
                }

                _aquarium.ProcessCycle();
            }
        }

        private void HandleAddFish()
        {
            if (_aquarium.FishesCount == _aquarium.Capacity)
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
                _aquarium.AddFish(new Fish(fishName));
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        private void HandleRemoveFish()
        {
            if (_aquarium.FishesCount == 0)
            {
                Console.WriteLine("No fishes in aquarium");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            _aquarium.ShowAllFishes();
            Console.Write("Enter fish index to remove: ");
            int.TryParse(Console.ReadLine(), out int input);

            int fishIndexToRemove = input - 1;

            if (_aquarium.TryRemoveFishAt(fishIndexToRemove, out Fish removedFish))
            {
                Console.WriteLine($"{removedFish.Name} has been removed from aquarium");
            }
            else
            {
                Console.WriteLine("Invalid fish index");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }
    }

    class Aquarium
    {
        private List<Fish> _fishes = new List<Fish>();

        public Aquarium()
        {
            Capacity = 5;
            Cycle = 0;
        }

        public int Capacity { get; private set; }
        public int Cycle { get; private set; }
        public int FishesCount => _fishes.Count;

        public void ProcessCycle()
        {
            Cycle++;

            AgeAllFishes();
            RemoveDeadFishes();
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Current cycle: {Cycle}\n");
            Console.WriteLine($"Aquarium's capacity: {Capacity}");

            ShowAllFishes();
        }

        public void ShowAllFishes()
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

        public void AddFish(Fish fish)
        {
            if (_fishes.Count < Capacity)
            {
                _fishes.Add(fish);

                Console.WriteLine($"Fish {fish.Name} has been added to aquarium");
            }
            else
            {
                Console.WriteLine("Aquarium is full");
            }
        }

        public bool TryRemoveFishAt(int index, out Fish removedFish)
        {
            if (index >= 0 && index < _fishes.Count)
            {
                removedFish = _fishes[index];
                _fishes.RemoveAt(index);
                return true;
            }

            removedFish = null;
            return false;
        }

        private void AgeAllFishes()
        {
            foreach (Fish fish in _fishes)
            {
                fish.Grow();
            }
        }

        private void RemoveDeadFishes()
        {
            for (int i = _fishes.Count - 1; i >= 0; i--)
            {
                Fish currentFish = _fishes[i];

                if (currentFish.Age >= currentFish.DeadAge)
                {
                    Console.WriteLine($"Fish {currentFish.Name} has died of old age ({currentFish.DeadAge} y.o)");
                    Console.ReadKey();

                    _fishes.Remove(currentFish);
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
