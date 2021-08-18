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


            if (method == TriggerQuery.TriggerType.DELETE)
            {
                ReplaceData("blacklist.json", docs);   
            }
            else
            {
                ReplaceData("whitelist.json", docs);
            }
        }

        private static void ReplaceData<T>(string filepath, IEnumerable<T> data)
        {
            string newJsonString = string.Empty;
            if (!File.Exists(filepath)) {
                File.Create(filepath).Dispose();
            }
            using(StreamReader r = new StreamReader(filepath))
            {
                var json = r.ReadToEnd();
                if (json.Length > 0)
                {
                    var oldJson = JsonSerializer.Deserialize<IEnumerable<T>>(json);
                    var newJson = oldJson.ToArray().Concat<T>(data);
                    newJsonString = JsonSerializer.Serialize(newJson);
                }
                else
                {
                    newJsonString = JsonSerializer.Serialize(data);
                }
                
            
            }
            File.WriteAllText(filepath, newJsonString);
        }
    }
}