using System.Collections.Generic;
using System.Collections;
using Searchify.Domain.Models;
using Searchify.Domain.Utils;
using System;
using System.Linq;



namespace Searchify.Services
{
    public class GenerateQueriesService
    {
        public static Response<IEnumerable<string>> DisplayResults(string query)
        {
            IEnumerable<string> data = Enumerable.Range(1, 10).Select((idx) =>
            {
                return "this is the query " + idx;
            });

            return new Response<IEnumerable<string>>(data, "These are the generated queries");

        }
    }
}