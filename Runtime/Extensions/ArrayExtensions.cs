using System;

namespace Stereome
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Add a value of T to input T[].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T[] Add<T>(this T[] input, T value)
        {
            int count = input.Length;
            T[] result = new T[count + 1];
            Array.Copy(input, result, count);
            result[count] = value;
            return result;
        }

        /// <summary>
        /// Remove a value of T from input T[].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T[] Remove<T>(this T[] input, T value)
        {
            int count = input.Length - 1;
            T[] result = new T[count];
            int index = Array.IndexOf(input, value);
            Array.Copy(input, 0, result, 0, index);
            Array.Copy(input, index + 1, result, index + 1, count - index);
            return result;
        }
    }
}
