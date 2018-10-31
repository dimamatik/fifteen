using System;

namespace Fifteen
{
    internal static class ArrayExtensions
    {
        private static void Check<T>(T[] array, int a, int b)
        {
            if (array == null)
                throw new NullReferenceException("Массив не может быть NULL");
            if (a < 0 || b < 0 || a >= array.Length || b >= array.Length)
                throw new ArgumentOutOfRangeException("Индексы массива " + a + " или " + b + " выходят за его границы");
        }
        /// <summary>
        /// Поменять местами элементы с индексами
        /// </summary>
        /// <returns>Возвращает ссылку на себя</returns>
        public static T[] Swap<T>(this T[] array, int a, int b)
        {
            Check(array, a, b);
            if (a == b) return array;
            
            T x = array[a];
            array[a] = array[b];
            array[b] = x;

            return array;
        }
        /// <summary>
        /// Циклически переставить элементы между индексами, если индексы допустимы, в направлении от a к b
        /// </summary>
        /// <returns>Возвращает ссылку на себя</returns>
        public static T[] Circle<T>(this T[] array, int a, int b)
        {
            Check(array, a, b);
            if (a == b) return array;

            if (a < b) for (int i = b; i > a; i--) array.Swap(i - 1, i);
            else for (int i = b; i < a; i++) array.Swap(i, i + 1);

            return array;
        }
    }
}
