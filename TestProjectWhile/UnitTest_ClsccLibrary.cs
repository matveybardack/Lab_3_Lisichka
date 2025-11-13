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
         class QuantifierFlowTests
        {
            [Fact(DisplayName = "UI_Flow: Корректное определение типа квантора и вызов сервиса для ∃")]
            public void BuildGraphButton_QuantifierFlow_ShouldCallEvaluateQuantifiedStatement()
            {
                // ACT: Имитируем входные данные
                string formulaInput = "∃x (x > 0)";
                double minInput = -2.0;
                double maxInput = 2.0;
                double stepInput = 1.0;

                // SETUP: Создаем заглушки
                var parserManager = new ParserManager();
                var plotService = new PlotPredicateService();

                // ИМИТАЦИЯ ЛОГИКИ ИЗ BuildGraphButton_Click

                // 1. Валидация ввода (Предполагаем, что она успешна)
                string formula = formulaInput;
                double min = minInput;
                double max = maxInput;
                double step = stepInput;

                // 2. Вызов Сервиса Парсера
                bool hasQuantifiers = parserManager.HasQuantifiers(formula); // true
                string ncalcText = parserManager.NormalizeToNCalc(formula); // "(x > 0)"

                // 3. Создание предиката
                // NCalcExpression в Predicate создается из ncalcText
                var ncalcExpr = new NCalc.Expression(ncalcText);
                var predicate = new Predicate(ncalcExpr, hasQuantifiers);

                // 4. Логика обработки кванторов
                if (hasQuantifiers)
                {
                    // ИСПОЛЬЗУЕМ ПРОГРАММНЫЙ ИНТЕРФЕЙС ДЛЯ ПОЛУЧЕНИЯ ТИПА КВАНТОРА:
                    PredicateAnalyzer.QuantifierEvaluationType quantifierEnum =
                        parserManager.GetQuantifierType(formula); // Existential

                    // ВЫЗОВ СЕРВИСА (Здесь мы проверяем, что вызов произошел, 
                    // и что мок вернул ожидаемый результат)
                    bool isTrue = plotService.EvaluateQuantifiedStatement(
                        predicate,
                        quantifierEnum,
                        min, max, step); // true

                    // ASSERT: Проверяем, что все сработало как ожидалось
                    Xunit.Assert.True(hasQuantifiers);
                    Xunit.Assert.Equal("(x > 0)", ncalcText);
                    Xunit.Assert.Equal(PredicateAnalyzer.QuantifierEvaluationType.Existential, quantifierEnum);
                    Xunit.Assert.True(isTrue); // Проверяем, что сервис вернул True
                }
                else
                {
                    Xunit.Assert.True(false, "Тест не должен попасть в ветку без кванторов.");
                }
            }
        }
    }
}
}
