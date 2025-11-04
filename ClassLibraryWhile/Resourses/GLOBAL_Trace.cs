namespace ClassLibraryWhile.Resourses
{
    public static class GLOBAL_Trace
    {
        public static Dictionary<string, LoopSpec> Modes = new()
        {
            ["PrefixSum"] = new LoopSpec
            {
                /// <summary>
                /// Первая инициализация инварианта
                /// </summary>
                /// <param name="i">инициализационное значение инварианта</param>
                /// <returns></returns>
                Inv_FirstInit = (_) => $"n > 0 ⇒ res = {0}",

                /// <summary>
                /// Сохранение инварианта
                /// </summary>
                /// <param name="len">длина массива</param>
                /// <param name="res">префиксная сумма</param>
                /// <param name="i">номер шага</param>
                /// <returns></returns>
                Save_Inv = (int len, int res, int i) => $"(res = {res} ^ {LoopSpec.VarFun(i, len)} >= 0) => {res} = {res}",

                /// <summary>
                /// Выход из инварианта
                /// </summary>
                /// <param name="len">длина массива</param>
                /// <param name="res">префиксная сумма</param>
                /// <param name="i">номер шага</param>
                /// <returns></returns>
                Exit = (int len, int res, int i) => $"(res = {res} ^ {LoopSpec.VarFun(i, len)} < 0) => {res} = {res}"
            },

            ["PrefixMax"] = new LoopSpec
            {
                /// <summary>
                /// Первая инициализация инварианта
                /// </summary>
                /// <param name="i">инициализационное значение инварианта</param>
                /// <returns></returns>
                Inv_FirstInit = (_) => $"n > 0 ⇒ res = {int.MinValue}",

                /// <summary>
                /// Сохранение инварианта
                /// </summary>
                /// <param name="len">длина массива</param>
                /// <param name="res">префиксная сумма</param>
                /// <param name="i">номер шага</param>
                /// <returns></returns>
                Save_Inv = (int len, int res, int i) => $"(res = {res} ^ {LoopSpec.VarFun(i, len)} >= 0) => {res} = {res}",

                /// <summary>
                /// Выход из инварианта
                /// </summary>
                /// <param name="len">длина массива</param>
                /// <param name="res">префиксная сумма</param>
                /// <param name="i">номер шага</param>
                /// <returns></returns>
                Exit = (int len, int res, int i) => $"(res = {res} ^ {LoopSpec.VarFun(i, len)} < 0) => {res} = {res}"
            }
        };
    }
}
