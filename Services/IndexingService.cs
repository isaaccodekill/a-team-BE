using System.Collections.Generic;
using Searchify.Domain.Models;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using System;




namespace Searchify.Services
{
    public class IndexingService
    {
        private readonly ILogger<IndexingService> _logger;


        public static void LogDocument(IEnumerable<Document> docs, TriggerQuery.TriggerType method)
        {


            var docsArray = docs.ToArray();

            for (var i = 0; i < docsArray.Length; i++)
            {
                Console.WriteLine(docsArray[i].url);
            }

        }

    }
}