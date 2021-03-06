using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Unicode;
using RestSharp;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Searchify.DynamoDb.Models;
using Document = Searchify.Domain.Models.Document;


namespace Searchify.Services
{
    public struct IndexerDocument
    {
        [JsonProperty("id")]
        public uint Id;

        [JsonProperty("url")]
        public string Url;
    }
    
    /// <summary>
    /// Class that handles the relaying to uploaded documents to indexing service
    /// </summary>
    public class IndexingService
    {
        
        private static HttpClient _client = new HttpClient();

        /// <summary>
        /// Static method that relays multiple books to the indexing service
        /// </summary>
        /// <param name="data">Document to be indexed</param>
        /// <returns> A boolean promise that determines if the relaying of documents was succesfull</returns>
        public static async Task<bool> Index(Document data, string urlString){
            Console.WriteLine("data", data.ToString()); 
            IndexerDocument[] documentPayload = new IndexerDocument[1];
            documentPayload[0] = new IndexerDocument { Id = (uint)data.id, Url = data.url };




            var request = new HttpRequestMessage()
            {
                RequestUri =  urlString != String.Empty ? new Uri(urlString) :  new Uri(Environment.GetEnvironmentVariable("INDEXER_URL")),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(documentPayload), Encoding.UTF8, "application/json")
            };
            
            
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(documentPayload));
            var response = await _client.SendAsync(request);

            Console.WriteLine(response.StatusCode);
            Console.WriteLine(JsonConvert.SerializeObject(documentPayload));
            if (response.IsSuccessStatusCode)
            {
                await InvertedIndexModel.SetLastId((uint) data.id);
                return true;
            }

            return false;
        }
        /// <summary>
        /// Static method that relays multiple books to the indexing service
        /// </summary>
        /// <param name="data"> List of documents to be indexed </param>
        /// <returns> A boolean promise that determines if the relaying of documents was succesfull </returns>
        public static async Task<bool> IndexMany(List<Document> data)
        {
            Console.WriteLine("data", data.ToString());
            IndexerDocument[] documentPayload = data.Select(doc => new IndexerDocument{ Id = (uint) doc.id, Url = doc.url }).ToArray();
            
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(Environment.GetEnvironmentVariable("INDEXER_URL")),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(documentPayload), Encoding.UTF8, "application/json")
            };
            
            
            var response = await _client.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                var last = data.OrderBy(doc => doc.id).Last();
                await InvertedIndexModel.SetLastId((uint)last.id);
                return true;
            }

            return false;
        }

    }    
}