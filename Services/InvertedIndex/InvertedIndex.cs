using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Searchify.DynamoDb.Models;


namespace Searchify.Services.InvertedIndex
{

    /// <summary>
    /// Indexer class builds and maintains internal search index
    /// </summary>
    public class Indexer
    {

        /// <summary>
        /// Last File ID indexed
        /// </summary>
        public uint LastId;


        public Dictionary<string, IndexTerm[]> ReverseIndex = new Dictionary<string, IndexTerm[]>();

        public async Task LoadInvertedIndex(string[] queryTerms)
        {
            await Task.WhenAll(queryTerms.Select(async t =>
            {
                IndexTerm[] terms = await GetIndexTermArray(t);
                ReverseIndex.Add(t, terms);
                return terms;
            }));
        }

        public IndexTerm[] GetLoadedTermList(string term)
        {
            return ReverseIndex[term];
        }

        public Indexer(uint lastId)
        {
            LastId = lastId;
        }



        /// <summary>
        /// Returns index list associated with <paramref name="word"/>
        /// </summary>
        /// <param name="word">any string</param>
        /// <returns>Index list of word</returns>
        public async Task<IndexTerm[]> GetIndexTermArray(string word)
        {
            List<IndexTerm> list = await InvertedIndexModel.GetIndexTermList(word);
            return list.ToArray();
        }

        // sums up filedeltas in a termlist
        private async Task<uint> SumDeltasInTermList(string word)
        {
            var terms = await GetIndexTermArray(word);
            return (uint)terms.Sum(indexTerm => indexTerm.FileDelta);
        }

    }
}