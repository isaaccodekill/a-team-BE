using System;

namespace Searchify.Services.Searcher
{
    /// <summary>
    /// Search Pointer object aids the k-way linear merge algorithm by storing the pointer to each index term being
    /// iterated reverse index
    /// </summary>
    public class Pointer : IComparable, IComparable<Pointer>
    {
        /// <summary>
        /// The key of the reverse index
        /// </summary>
        public readonly string Term;
        /// <summary>
        /// Index of current iteration of the Index Term list
        /// </summary>
        public readonly uint P;
        /// <summary>
        /// FileId of Index Term
        /// </summary>
        public readonly uint FileId;

        /// <summary>
        /// Instantiate a Pointer object
        /// </summary>
        /// <param name="term">word</param>
        /// <param name="p">current index of IndexTermList iteration</param>
        /// <param name="fileId">file id</param>
        public Pointer(string term, uint p, uint fileId)
        {
            P = p;
            Term = term;
            FileId = fileId;
        }

        /// <summary>
        /// IComparable implementation
        /// </summary>
        /// <param name="other">object for comparison</param>
        /// <returns>
        /// 1 when this.FileId gt other.FileId
        /// -1 when this.FileId lt other.FileId
        /// 0 otherwise
        /// </returns>
        /// <exception cref="ArgumentException">Raised when comparing with null</exception>
        public int CompareTo(object other)
        {
            if (other == null)
            {
                throw new ArgumentException("cannot compare Pointer to null");
            }

            Pointer pointer = (Pointer)other;
            if (FileId == pointer.FileId) return 0;
            if (FileId < pointer.FileId) return -1;
            return 1;
        }

        public int CompareTo(Pointer other)
        {
            if (other.GetType() != typeof(Pointer))
            {
                throw new ArgumentException("cannot compare Pointer to " + other.GetType());
            }
            if (ReferenceEquals(this, other)) return 0;
            if (FileId == other.FileId) return 0;
            if (FileId < other.FileId) return -1;
            return 1;
        }
    }
}