using System.Collections.Generic;
using System.Collections;
using Searchify.Domain.Models;
using Searchify.Domain.Utils;
using System;
using System.Linq;



namespace Searchify.Services
{
    public class SearchService
    {
        public static Response<IEnumerable<SearchHit>> DisplayResults(string query)
        {
            IEnumerable<SearchHit> data = Enumerable.Range(1, 10).Select((idx) =>
            {
                Document doc = new Document();
                return new SearchHit(doc, "this is the preview text for the book");
            });

            return new Response<IEnumerable<SearchHit>>(data, "These are the reponses for the query " + query);
             
        }
    }
}