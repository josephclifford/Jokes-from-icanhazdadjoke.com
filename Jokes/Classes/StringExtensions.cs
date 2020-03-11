using System;

namespace Jokes.Classes
{
    public static class StringExtensions
    {
        /// <summary>
        /// Counts the number of words in a string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int WordCount(this string text)
        {
            return text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}
