using System;
using System.Collections.Generic;
using System.Linq;
using MoreComplexDataStructures;
using Searchify.Services.Searcher;
using Searchify.Services.InvertedIndex;

namespace Searchify.Services
{

    /// <summary>
    /// Maintains and calculates document scores for a query
    /// </summary>
    public class Ranker
    {
        private Indexer _indexer;
        private Dictionary<uint, double> _scores = new Dictionary<uint, double>();

        /// <summary>
        /// Instantiates a ranker object
        /// </summary>
        /// <param name="indexer">an instance of <see cref="Indexer"/></param>
        public Ranker(Indexer indexer)
        {
            _indexer = indexer;
        }

        /// <summary>
        /// Computes and stores file score
        /// </summary>
        /// <param name="fieldId">id of file</param>
        /// <param name="pointerList">pointer list of query terms that can be found in the file</param>
        public void Score(uint fieldId, List<Pointer> pointerList)
        {
            _tfIdfScore(fieldId, pointerList);
        }

        /// <summary>
        /// Returns an ordered array of file ids based on scores
        /// </summary>
        /// <returns>list of file ids</returns>
        public uint[] RankedResultsList()
        {
            string text = "";
            foreach (KeyValuePair<uint, double> kvp in _scores)
            {
                text += $"Key = {kvp.Key}, Value = {kvp.Value}\n";
            }
            Console.WriteLine(text);

            return _scores.OrderByDescending(kvp => kvp.Value)
                .Select(kvp => kvp.Key)
                .ToArray();
        }

        public void ClearScores()
        {
            _scores = new Dictionary<uint, double>();
        }

        // TF IDF Scoring
        private void _tfIdfScore(uint fileId, List<Pointer> pointerList)
        {
            double total = 0;
            //
            MinHeap<ulong> positions = new MinHeap<ulong>();
            //

            foreach (var pointer in pointerList)
            {   
                // normalized term frquency: number of occurences of term in document
                double tf = _indexer.GetLoadedTermList(pointer.Term)[pointer.P].Frequency;
                
                Console.WriteLine(tf);
                
                //
                uint currentPos = 0;
                uint[] pos = _indexer.GetLoadedTermList(pointer.Term)[pointer.P].Positions;
                
                // Console.WriteLine(string.Join(' ', pos));
                
                foreach (var posi in pos)
                {
                    currentPos += posi;
                    positions.Insert(currentPos);
                }
                //

                // inverse document frquency: lg(N / df(t))
                // N : total number of documents
                // df(t): total number of documents that have term t
                double idf = Math.Log(_indexer.LastId / (double)_indexer.GetLoadedTermList(pointer.Term).Length, 2);
                
                Console.WriteLine(idf);
                Console.WriteLine((double)_indexer.GetLoadedTermList(pointer.Term).Length);
                Console.WriteLine(_indexer.LastId);

                total += tf * idf;
            }

            //
            int largestConsecutive = 0;
            int consecutive = 0;
            ulong current = positions.ExtractMin();
            while (positions.Count > 1)
            {
                ulong next = positions.ExtractMin();
                if (next - current <= 1)
                {
                    consecutive += 1;
                    if (consecutive > largestConsecutive)
                    {
                        largestConsecutive = consecutive;
                    }
                }
                else
                {
                    consecutive = 0;
                }

                current = next;
            }

            //
            _scores.Add(fileId, total + largestConsecutive);
        }
    }
}