using System;
using System.Threading;

namespace Site
{
    /// <summary>
    /// Потокобезопасный ГСЧ.
    /// </summary>
    public class MyRandom
    {
        private static int _seed = Environment.TickCount;
        private static ThreadLocal<Random> _random = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref _seed)));
        /// <summary>
        /// Случайное число в указанном диапазоне.
        /// </summary>
        /// <param name="Max">Максимальное значение</param>
        /// <returns>Случайное число</returns>
        public static int Next(int Max)
        {
            return _random.Value.Next(0, Max);
        }
        /// <summary>
        /// Случайное число в указанном диапазоне.
        /// </summary>
        /// <param name="Min">Минимальное значение</param>
        /// <param name="Max">Максимальное значение</param>
        /// <returns>Случайное число</returns>
        public static int Next(int Min, int Max)
        {
            return _random.Value.Next(Min, Max);
        }
    }
}