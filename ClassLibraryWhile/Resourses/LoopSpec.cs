using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryWhile.Resourses
{
    public class LoopSpec
    {
        public Func<int, string> Inv_FirstInit;
        public Func<int, int, int, string> Save_Inv; // Проверка инварианта на шаге
        public Func<int, int, int, string> Exit;// Проверка постусловия
        public static int VarFun(int len, int i = 0) => len - i; // Вычисление t
    }
}
