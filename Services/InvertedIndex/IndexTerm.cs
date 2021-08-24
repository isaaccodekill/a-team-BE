using System;
using System.IO;
using System.Linq;
using Searchify;
namespace Searchify.Services.InvertedIndex
{
    /// <summary>
    /// Index term representation, stores file delta, frequency and positions of word
    /// </summary>
    public class IndexTerm
    {

        // internal rep of fileDelta
        private readonly MemoryStream _frequencyStream = new MemoryStream();

        public readonly uint FileDelta;

        // internal rep of positions array
        private readonly MemoryStream _positionsStream = new MemoryStream();

        // public uint[] Positions;
        // public uint Frequency;

        public IndexTerm(uint fileDelta)
        {
            FileDelta = fileDelta;
        }

        public void AddPositions(uint[] positions)
        {
            Config.Codec.EncodeSingle(_frequencyStream, (ulong)positions.Length);
            Config.Codec.EncodeMany(_positionsStream, positions.Select(i => (ulong)i).ToArray());
        }

        public uint[] Positions
        {
            get
            {
                ulong[] values = new ulong[Frequency];
                _positionsStream.Seek(0, SeekOrigin.Begin);
                Config.Codec.DecodeMany(_positionsStream, values);
                return values.Select(i => (uint)i).ToArray();
            }
        }

        public uint Frequency
        {
            get
            {
                _frequencyStream.Seek(0, SeekOrigin.Begin);
                ulong value = Config.Codec.DecodeSingle(_frequencyStream);
                return (uint)value;
            }
        }
    }
}