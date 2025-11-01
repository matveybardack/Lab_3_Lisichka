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
            int[] ints = new int[] { 1, 2, 3, 4, 5 };
            ClassLibraryWhile.PrefixSum prefixSum = new ClassLibraryWhile.PrefixSum();
            foreach (var (sum, index) in prefixSum.GetPrefixSums(ints))
            {
                Console.WriteLine($"Index: {index}, Prefix Sum: {sum}");
            }
        }
    }
}
