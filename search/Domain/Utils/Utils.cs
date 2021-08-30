using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Searchify.Domain.Utils
{
    /// <summary>
    /// Static class housing a number of utility methods
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Converts multiple spaces to one an strips punctuation from text, converts text to lowercase
        /// </summary>
        /// <param name="text">any string value</param>
        /// <returns>cleaned text</returns>
        public static string CleanText(string text)
        {
            text = Regex.Replace(text, @"\s+", " ");
            text = Regex.Replace(text, "[^A-Za-z0-9 ]", "");
            text = new string(text.Where(c => !char.IsPunctuation(c)).ToArray());
            return text.ToLower();
        }

        /// <summary>
        /// Creates a list where elements are replaced by the value of the delta between each element and the previous element
        /// </summary>
        /// <param name="list">list of nonnegative integers</param>
        /// <returns>list of delta ulong values</returns>
        public static List<ulong> ToDeltaList(List<int> list)
        {
            List<ulong> output = new List<ulong>();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    output.Add((ulong)list[i]);
                    continue;
                }
                output.Add((ulong)(list[i] - list[i - 1]));
            }

            return output;
        }
    }
}