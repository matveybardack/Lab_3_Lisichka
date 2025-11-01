namespace ConsoleApp1
{
    enum HH
    {
        gg,
        ff,
        cc
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"{HH.gg > HH.cc}");
            bool [] bb = { true, false };

            Console.WriteLine("A B C Res");
            foreach (var A in bb)
                foreach (var B in bb)
                    foreach (var C in bb)
                    {
                        bool i = !(C || B) && A;
                        Console.WriteLine($"{A} {B} {C} {i}");
                    }
        }
    }
}
