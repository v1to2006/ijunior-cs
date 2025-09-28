namespace Assignments.OOP.AutoService
{
    class Mechanic
    {
        public AutoService _autoService;

        public Mechanic()
        {
            AutoServiceFactory autoServiceFactory = new AutoServiceFactory();
        }

        public void Run()
        {
            const string CommandRepair = "repair";
            const string CommandExit = "exit";

            bool isContinue = true;

            while (isContinue)
            {
                Console.Clear();
                Console.Write("\n> ");
                string command = Console.ReadLine();

                switch (command.ToLower())
                {
                    case CommandRepair:
                        break;
                    case CommandExit:
                        isContinue = false;
                        break;
                    default:
                        Console.WriteLine("Unknown command.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
