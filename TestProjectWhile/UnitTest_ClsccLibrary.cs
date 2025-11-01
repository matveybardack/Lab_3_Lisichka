using ClassLibraryWhile;

namespace TestProjectWhile
{
    public class UnitTest_ClsccLibrary
    {
        [Fact]
        public void Test_GetPrefixSum_Indexes()
        {
            // на вход
            int[] ints = { 1, 2, 3 };

            // ожидаемый
            int[] index = {1, 2, 3 };

            var x = new PrefixSum();
            var get_index =  x.GetPrefixSums(ints).Select(t => t.Item2).ToArray();

            Assert.Equal(index, get_index);

        }

        [Fact]
        public void Test_GetPrefixSum_Sums()
        {
            // на вход
            int[] ints = { 1, 2, 3 };

            // ожидаемый
            int[] res = { 1, 3, 6 };

            var x = new PrefixSum();
            var get_res = x.GetPrefixSums(ints).Select(t => t.Item1).ToArray();

            Assert.Equal(res, get_res);

        }
    }
}
