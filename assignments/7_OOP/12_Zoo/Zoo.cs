namespace Assignments.OOP.Zoo
{
    enum Gender
    {
        Male,
        Female
    }

    class ZooFactory
    {
        private CageFactory _cageFactory = new CageFactory();

        public Zoo Create()
        {
            return new Zoo(new List<Cage>
            {
                _cageFactory.Create("Lion", "Roar!"),
                _cageFactory.Create("Pig", "Oink!"),
                _cageFactory.Create("Horse", "Whinny!"),
                _cageFactory.Create("Owl", "Hoot!"),
            });
        }
    }

    class CageFactory
    {
        private AnimalFactory _animalFactory = new AnimalFactory();

        public Cage Create(string animalType, string animalSound)
        {
            List<Animal> animals = new List<Animal>();

            int minAnimalsCount = 2;
            int maxAnimalsCount = 5;
            int animalsCount = Utils.GetRandomNumber(minAnimalsCount, maxAnimalsCount + 1);

            for (int i = 0; i < animalsCount; i++)
            {
                animals.Add(_animalFactory.Create(animalType, animalSound));
            }

            return new Cage(animals);
        }
    }

    class AnimalFactory
    {
        public Animal Create(string animalType, string animalSound)
        {
            return new Animal(animalType, animalSound, GenerateRandomGender());
        }

        private Gender GenerateRandomGender()
        {
            Gender[] genders = { Gender.Male, Gender.Female };

            return genders[Utils.GetRandomNumber(0, genders.Length)];
        }
    }

    class Visitor
    {
        private Zoo _zoo;

        public Visitor()
        {
            ZooFactory zooFactory = new ZooFactory();

            _zoo = zooFactory.Create();
        }

        public void Run()
        {
            const string CommandExit = "EXIT";

            bool isWorking = true;

            while (isWorking)
            {
                Console.Clear();
                Console.WriteLine("Welcome to Zoo!\n");

                _zoo.ShowCageList();

                Console.WriteLine($"\nEnter cage number to see info or type {CommandExit} to leave");
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
        private List<Cage> _cages;

        public Zoo(List<Cage> cages)
        {
            _cages = cages;
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
            cage.ShowDetails();
        }

        public void ShowCageList()
        {
            Console.WriteLine("Available cages in zoo:");

            for (int i = 0; i < _cages.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_cages[i].GetBasicInfo()}");
            }
        }
    }

    class Cage
    {
        private List<Animal> _animals = new List<Animal>();

        public Cage(List<Animal> animals)
        {
            _animals = animals;
        }

        public string GetBasicInfo()
        {
            if (_animals.Count == 0)
            {
                return "Empty cage";
            }

            return $"{_animals[0].Type}s cage";
        }

        public void ShowDetails()
        {
            if (_animals.Count == 0)
            {
                return;
            }

            string animalsType = _animals[0].Type;
            string animalsSound = _animals[0].Sound;

            Console.WriteLine($"{animalsType}s cage");
            Console.WriteLine($"Amount: {_animals.Count}");
            Console.WriteLine($"Sound: {animalsSound}");
            Console.WriteLine($"\nAll {animalsType}s in cage:\n");

            for (int i = 0; i < _animals.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_animals[i].GetInfo()}");
            }
        }
    }

    class Animal
    {
        private Gender _gender;

        public Animal(string animalType, string sound, Gender gender)
        {
            _gender = gender;
            Type = animalType;
            Sound = sound;
        }

        public string Type { get; private set; }
        public string Sound { get; private set; }

        public string GetInfo()
        {
            return $"{Type} - Gender: {_gender} - Sound: {Sound}";
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
