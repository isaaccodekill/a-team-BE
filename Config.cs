using System;
using System.IO;
using InvertedTomato.Compression.Integers;
namespace Searchify
{
    /// <summary>
    /// Application Constants
    /// </summary>
    public static class Config
    {
        public static readonly string DatabaseHost = Environment.GetEnvironmentVariable("DATABASE_HOST") ?? "localhost";
        public static readonly int DatabasePort = Environment.GetEnvironmentVariable("DATABASE_PORT") != null ? Convert.ToInt32(Environment.GetEnvironmentVariable("DATABASE_PORT")) : 8000;
        public static readonly Codec Codec = new FibonacciCodec();
    }
}