using System.Collections.Generic;
using System.Linq;

namespace Searchify.Domain.Utils
{
    public static class Helpers
    {
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