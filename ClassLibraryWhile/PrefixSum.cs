using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryWhile
{
    public class PrefixSum
    {
        public void Step(int[] arr, ref int i, ref int res)
        {
            res += arr[i++];
        }

        public void Play(int[] arr, ref int i, ref int res)
        {
            while (i < arr.Length)
            {
                Step(arr, ref i, ref res);
            }
        }
    }
}
