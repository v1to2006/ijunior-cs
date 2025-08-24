namespace OOP.Assignments.Zoo
{
    enum Gender
    {
        Male,
        Female
    }

    class Visitor
    {
        private Zoo _zoo = new Zoo();

        public void Run()
        {
            const string CommandExit = "EXIT";

            bool isWorking = true;

            while (isWorking)
            {
                Console.Clear();
                Console.WriteLine("Welcome to Zoo!\n");

                Console.WriteLine("All cages:\n");
                _zoo.ShowCages();

                Console.WriteLine("\nEnter cage number to see info or type EXIT to leave");
                Console.Write("> ");
                string userInput = Console.ReadLine().ToUpper();

                Console.Clear();

                switch (userInput)
                {
                    case CommandExit:
                        isWorking = false;
                        break;
                    default:
                        VisitCage(userInput);
                        break;
                }
            }
        }

        public void VisitCage(string userInput)
        {
            if (!int.TryParse(userInput, out int cageIndex))
            {
                return;
            }

            if (_zoo.TryGetCageByIndex(cageIndex - 1, out Cage cage))
            {
                _zoo.ShowCageInfo(cage);
            }
            else
            {
                Console.WriteLine("Cage not found");
            }

            Console.WriteLine("\nPress any key to return");
            Console.ReadKey();
        }
    }

    class Zoo
    {
        List<Cage> _cages = new List<Cage>();

        public Zoo()
        {
            InitCages();
        }

        public bool TryGetCageByIndex(int index, out Cage cage)
        {
            if (index >= 0 && index < _cages.Count)
            {
                cage = _cages[index];
                return true;
            }
            else
            {
                cage = null;
                return false;
            }
        }

        public void ShowCageInfo(Cage cage)
        {
            cage.ShowInfo();
        }

        public void ShowCages()
        {
            Console.WriteLine("Available cages in zoo:");

            for (int i = 0; i < _cages.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_cages[i].AnimalType}s cage");
            }
        }

        private void InitCages()
        {
            _cages.Add(new Cage("Lion", "Roar!"));
            _cages.Add(new Cage("Pig", "Oink!"));
            _cages.Add(new Cage("Horse", "Whinny!"));
            _cages.Add(new Cage("Owl", "Hoot!"));
        }
    }

    class Cage
    {
        private List<Animal> _animals = new List<Animal>();

        public Cage(string animalType, string animalSound)
        {
            AnimalType = animalType;
            AnimalSound = animalSound;

            InitAnimals(animalType, animalSound);
        }

        public string AnimalType { get; private set; }
        public string AnimalSound { get; private set; }

        public void ShowInfo()
        {
            Console.WriteLine($"{AnimalType}s cage");
            Console.WriteLine($"Amount: {_animals.Count}");
            Console.WriteLine($"Sound: {AnimalSound}");
            Console.WriteLine($"\nAll {AnimalType}s in cage:\n");

            for (int i = 0; i < _animals.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_animals[i].GetInfo()}");
            }
        }

        private void InitAnimals(string animalType, string animalSound)
        {
            int minAnimalsCount = 2;
            int maxAnimalsCount = 5;

            int animalsCount = Utils.GetRandomNumber(minAnimalsCount, maxAnimalsCount + 1);

            for (int i = 0; i < animalsCount; i++)
            {
                _animals.Add(new Animal(animalType, animalSound));
            }
        }
    }

    class Animal
    {
        public Animal(string animalType, string sound)
        {
            AnimalType = animalType;
            Gender = GetRandomGender();
            Sound = sound;
        }

        public string AnimalType { get; private set; }
        public Gender Gender { get; private set; }
        public string Sound { get; private set; }

        public string GetInfo()
        {
            return $"{AnimalType} - {Gender} - {Sound}";
        }

        private Gender GetRandomGender()
        {
            int genderValue = Utils.GetRandomNumber(0, 1 + 1);

            if (genderValue == 0)
            {
                return Gender.Male;
            }
            else
            {
                return Gender.Female;
            }
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
