using System.Collections.Generic;
using System.Linq;

namespace Searchify.Domain.Utils
{
    /// <summary>
    /// Static class that houses logic for removing stop words from a query
    /// </summary>
    public static class Stopwords
    {
        /// <summary>
        /// Creates a Hashset of possible stop words in the english vocabulary
        /// </summary>
        /// <returns> A set of english stop words </returns>
        public static HashSet<string> LoadStopWords()
        {
            string stopwords = "i me my myself we our ours ourselves you youre youve youll youd your yours yourself yourselves he him his himself she shes her hers herself it its its itself they them their theirs themselves what which who whom this that thatll these those am is are was were be been being have has had having do does did doing a an the and but if or because as until while of at by for with about against between into through during before after above below to from up down in out on off over under again further then once here there when where why how all any both each few more most other some such no nor not only own same so than too very s t can will just don dont should shouldve now d ll m o re ve y ain aren arent couldn couldnt didn didnt doesn doesnt hadn hadnt hasn hasnt haven havent isn isnt ma mightn mightnt mustn mustnt needn neednt shan shant shouldn shouldnt wasn wasnt weren werent wont wouldn wouldnt";
            return new HashSet<string>(stopwords.Split(' '));
        }

        /// <summary>
        /// Takes a string and removes the stop words in the string
        /// </summary>
        /// <param name="word">string from which to remove stop words</param>
        /// <returns>String without stop words</returns>
        public static List<string> Clean(string word)
        {
            HashSet<string> stopWords = LoadStopWords();
            var tokens = new List<string>(word.Trim().Split(null));
            var extractedToken = tokens.Where(a => !stopWords.Contains(a) && word != string.Empty);
            
            return extractedToken.ToList();
        }


        /// <summary>
        /// Helper method to determine if a string contains valid tokens that are not stop words
        /// </summary>
        /// <param name="queryTokens"> List of query tokes </param>
        /// <param name="suggestionTokens"> String </param>
        /// <returns>boolean to determine if string is a valid suggestion based on query tokens or not</returns>
        public static bool compareQuery(List<string> queryTokens, string suggestionTokens)
        {
            bool valid = true;

            for (var i = 0; i < queryTokens.Count(); i++)
            {
                if (!suggestionTokens.Contains(queryTokens[i]))
                {
                    valid = false;
                    break;
                }
            }

            return valid;
        }

    }
}