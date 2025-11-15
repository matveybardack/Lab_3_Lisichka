using Xunit;
using ClassLibraryWhile;
using ClassLibraryWhile.Resourses;
using System;
using System.Linq;

namespace ClassLibraryWhile.Tests
{
    public class PrefixTests
    {
        [Fact(DisplayName = "Проверка генерации инвариантов PrefixSum")]
        public void PrefixSum_InvStrings_ShouldBeCorrect()
        {
            var spec = GLOBAL_Trace.Modes["PrefixSum"];

            string init = spec.Inv_FirstInit(0);
            Assert.Contains("res = 0", init);

            string save = spec.Save_Inv(5, 10, 2);
            Assert.Contains("res = 10", save);
            Assert.Contains("=>", save);

            string exit = spec.Exit(5, 10, 2);
            Assert.Contains("res = 10", exit);
        }

        [Fact(DisplayName = "Проверка генерации инвариантов PrefixMax")]
        public void PrefixMax_InvStrings_ShouldBeCorrect()
        {
            var spec = GLOBAL_Trace.Modes["PrefixMax"];

            string init = spec.Inv_FirstInit(0);
            Assert.Contains($"res = {int.MinValue}", init);

            string save = spec.Save_Inv(5, 7, 2);
            Assert.Contains("res = 7", save);

            string exit = spec.Exit(5, 7, 2);
            Assert.Contains("res = 7", exit);
        }

        [Fact(DisplayName = "PrefixSum: корректность вычисления суммы и итератора")]
        public void PrefixSum_ShouldReturnCorrectPrefixSums()
        {
            var arr = new[] { 1, 2, 3, 4 };
            var ps = new PrefixSum();

            var results = ps.GetPrefixSums(arr).ToList();

            int[] expectedSums = { 1, 3, 6, 10 };
            for (int i = 0; i < arr.Length; i++)
            {
                Assert.Equal(expectedSums[i], results[i].Item1);
                Assert.Equal(i + 1, results[i].Item2);
            }
        }

        [Fact(DisplayName = "PrefixSum: метод Play должен корректно подсчитывать сумму")]
        public void PrefixSum_Play_ShouldComputeFullSum()
        {
            int[] arr = { 5, 10, -2 };
            int i = 0;
            int res = 0;

            var ps = new PrefixSum();
            ps.Play(arr, ref i, ref res);

            Assert.Equal(arr.Sum(), res);
            Assert.Equal(arr.Length, i);
        }

        [Fact(DisplayName = "PrefixMax: корректность вычисления максимума и итератора")]
        public void PrefixMax_ShouldReturnCorrectPrefixMax()
        {
            var arr = new[] { -5, 2, 7, 1 };
            var pm = new PrefixMax();

            var results = pm.GetPrefixSums(arr).ToList();

            int[] expectedMax = { -5, 2, 7, 7 };
            for (int i = 0; i < arr.Length; i++)
            {
                Assert.Equal(expectedMax[i], results[i].Item1);
                Assert.Equal(i + 1, results[i].Item2);
            }
        }

        [Fact(DisplayName = "VarFun должна возвращать корректное значение t = len - i")]
        public void VarFun_ShouldComputeVariantCorrectly()
        {
            int len = 10;
            Assert.Equal(10, LoopSpec.VarFun(len, 0));
            Assert.Equal(5, LoopSpec.VarFun(len, 5));
            Assert.Equal(0, LoopSpec.VarFun(len, 10));
        }

        [Fact(DisplayName = "GLOBAL_Trace должен содержать оба режима")]
        public void GlobalTrace_ShouldContainBothModes()
        {
            Assert.True(GLOBAL_Trace.Modes.ContainsKey("PrefixSum"));
            Assert.True(GLOBAL_Trace.Modes.ContainsKey("PrefixMax"));
        }
    }
}
