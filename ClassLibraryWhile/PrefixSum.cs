using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryWhile
{
    public class PrefixSum
    {
        /// <summary>
        /// Шаг итерации
        /// </summary>
        /// <param name="arr">массив</param>
        /// <param name="i">индекс элемента</param>
        /// <param name="res">префиксная сумма</param>
        private void Step(int[] arr, ref int i, ref int res)
        {
            res += arr[i++];
        }

        public void Play(int[] arr, ref int i, ref int res)
        {
            Debug.Assert(res == 0);
            while (i < arr.Length)
            {
                Step(arr, ref i, ref res);
            }
        }

        /// <summary>
        /// Получение префиксных сумм с помощью итератора
        /// </summary>
        /// <param name="arr"> массив </param>
        /// <returns>кортеж из префиксной суммы на шаге и номера элемента в массиве(начиная с 1)</returns>
        public IEnumerable<(int, int)> GetPrefixSums(int[] arr)
        {
            int i = 0;
            int res = 0;

            // Инвариант после шага: res = сумма первых j элементов
            // (проверка невычислима напрямую без повторного суммирования; оставляем как комментарий-инвариант)

            Debug.Assert(res == 0);

            while (i < arr.Length)
            {
                Step(arr, ref i, ref res);
                yield return (res, i);
            }
        }
    }
}
