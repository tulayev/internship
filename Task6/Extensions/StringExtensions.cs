using System;
using Task6.Utils;

namespace Task6.Extensions
{
    public static class StringExtensions
    {
        private static readonly Random rnd = new Random();

        public static string RemoveChar(this string s, int position) => s.Length > 0 ? s.Remove(position, 1) : s;

        public static string AddChar(this string s, int position, string culture)
        {
            string alphabet = Locales.Alphabet[culture];
            return s.Insert(position, alphabet[rnd.Next(0, alphabet.Length)].ToString());
        }

        public static string SwapChars(this string s, int p1, int p2)
        {
            if (s.Length > 1)
            {
                char[] array = s.ToCharArray();
                char temp = array[p1];
                array[p1] = array[p2];
                array[p2] = temp;
                return new String(array);
            }

            return s;
        }
    }
}
