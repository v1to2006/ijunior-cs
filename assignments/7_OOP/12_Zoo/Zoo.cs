namespace Assignments.OOP.Zoo
{
    enum Gender
    {
        Male,
        Female
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

            return new Cage(animalType, animalSound, animals);
        }
    }

    class AnimalFactory
    {
        public Animal Create(string animalType, string animalSound)
        {
            return new Animal(animalType, animalSound, GetRandomGender());
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
            cage.ShowFullInfo();
        }

        public void ShowCages()
        {
            Console.WriteLine("Available cages in zoo:");

            for (int i = 0; i < _cages.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_cages[i].GetBasicInfo()}");
            }
        }

        private void InitCages()
        {
            CageFactory factory = new CageFactory();

            _cages.Add(factory.Create("Lion", "Roar!"));
            _cages.Add(factory.Create("Pig", "Oink!"));
            _cages.Add(factory.Create("Horse", "Whinny!"));
            _cages.Add(factory.Create("Owl", "Hoot!"));
        }
    }

    class Cage
    {
        private string _animalType;
        private string _animalSound;
        private List<Animal> _animals = new List<Animal>();

        public Cage(string animalType, string animalSound, List<Animal> animals)
        {
            _animalType = animalType;
            _animalSound = animalSound;
            _animals = animals;
        }

        public string GetBasicInfo()
        {
            return $"{_animalType}s cage";
        }

        public void ShowFullInfo()
        {
            Console.WriteLine($"{_animalType}s cage");
            Console.WriteLine($"Amount: {_animals.Count}");
            Console.WriteLine($"Sound: {_animalSound}");
            Console.WriteLine($"\nAll {_animalType}s in cage:\n");

            for (int i = 0; i < _animals.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_animals[i].GetInfo()}");
            }
        }
    }

    class Animal
    {
        private string _type;
        private Gender _gender;
        private string _sound;

        public Animal(string animalType, string sound, Gender gender)
        {
            _type = animalType;
            _gender = gender;
            _sound = sound;
        }

        public string GetInfo()
        {
            return $"{_type} - Gender: {_gender} - Sound: {_sound}";
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
