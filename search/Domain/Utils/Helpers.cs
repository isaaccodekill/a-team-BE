using System.Collections.Generic;
using System.Linq;

namespace Searchify.Domain.Utils
{
    /// <summary>
    /// Helper class that marks auto complete suggestions to indicate relevance to original query
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Method to return 
        /// </summary>
        /// <param name="token">List of strings to mark</param>
        /// <param name="sentence"> Suggestion string to marked </param>
        /// <returns>Resultant string with html <mark/> markings </returns>
        public static string MarkSuggestions(List<string> token, string sentence)
        {
            var wordList = sentence.Split(null).Select(s => {

                if (token.Contains(s))
                {
                    return "<mark> " + s +  " </mark>";
                }

                return s;
            });

            return string.Join(" ", wordList);
        }
    }
}