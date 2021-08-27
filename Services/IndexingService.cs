using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using RestSharp;
using System.Threading.Tasks;



namespace Searchify.Services
{
    public class IndexingService
    {

        private static IRestClient client = new RestClient(Environment.GetEnvironmentVariable("INDEXER_URL"));
        private static IRestRequest request = new RestRequest("post", Method.POST);


        public static bool CallIndexer(Object data){

            IRestResponse response =  client.Execute(request.AddJsonBody(data));
            if (response.IsSuccessful)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }    
}