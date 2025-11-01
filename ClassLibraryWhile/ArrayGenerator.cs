namespace ClassLibraryWhile
{
    public static class ArrayGenerator
    {
        /// <summary>
        /// Генерация массива длины n в диапазоне от min до max
        /// </summary>
        /// <param name="n"> длина массива </param>
        /// <param name="min"> нижняя граница </param>
        /// <param name="max"> верхняя граница </param>
        /// <returns> массив </returns>
        public static int [] GenerateArray(int n, int min, int max)
        {
            Random r = new Random();

            int [] res_arr = new int[n];

            for (int i = 0; i < n; i++)
                res_arr[i] = r.Next(min, max);

            return res_arr;
        }


    }
}
