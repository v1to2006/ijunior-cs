namespace Assignments.OOP.AutoService
{
    static class Utils
    {
        private static Random _random = new Random();

        public static int GetRandomNumber(int minNumber, int maxNumber)
        {
            return _random.Next(minNumber, maxNumber);
        }
    }
}
