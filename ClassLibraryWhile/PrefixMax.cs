using ClassLibraryWhile.Resourses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryWhile
{
    public class PrefixMax
    {
        const int MinValue = int.MinValue;

        /// <summary>
        /// Шаг итерации
        /// </summary>
        /// <param name="arr">массив</param>
        /// <param name="i">индекс элемента</param>
        /// <param name="res">префиксный максимум</param>
        private void Step(int[] arr, ref int i, ref int res)
        {
            res = Math.Max(res, arr[i++]);
            GLOBAL_Trace.Modes["PrefixMax"].Save_Inv(arr.Length, i, res);
            GLOBAL_Trace.Modes["PrefixMax"].Exit(arr.Length, i, res);
        }

        public void Play(int[] arr, ref int i, ref int res)
        {
            Debug.Assert(res == int.MinValue);
            while (i < arr.Length)
            {
                Step(arr, ref i, ref res);
            }
        }

        /// <summary>
        /// Получение префиксного максимума с помощью итератора
        /// </summary>
        /// <param name="arr"> массив </param>
        /// <returns>кортеж из префиксного максимума на шаге и номера элемента в массиве(начиная с 1)</returns>
        public IEnumerable<(int, int)> GetPrefixSums(int[] arr)
        {
            int i = 0;
            int res = MinValue;
            GLOBAL_Trace.Modes["PrefixMax"].Save_Inv(arr.Length, i, res);

            // Инвариант после шага: res = максимум из первых j элементов
            // (проверка невычислима напрямую без повторного вычисления; оставляем как комментарий-инвариант)

            Debug.Assert(res == int.MinValue);

            while (i < arr.Length)
            {
                Step(arr, ref i, ref res);
                yield return (res, i);
            }
        }
    }
}
