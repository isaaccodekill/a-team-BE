using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Searchify.Domain.Models;
using Searchify.Domain.Utils;
using Searchify.Services;
using Microsoft.EntityFrameworkCore;
using Searchify.Services.Searcher;
using Searchify.Services.InvertedIndex;
using System.Diagnostics;
using Searchify.DynamoDb.Models;

namespace Searchify.Controllers
{

    /// <summary>
    /// Search controller class 
    /// </summary>
    [ApiController]
    [Route("/api/search")]
    public class SearchController : ControllerBase
    {

        private readonly SearchifyContext searchifyContext;
        private readonly Searcher _searcher;

        /// <summary>
        /// Search controller initializer
        /// </summary>
        public SearchController(SearchifyContext context, bool testMode)
        {
            searchifyContext = context;
            if (!testMode)
            {
                Indexer index = Task.Run(async () => {
                    return await LoadIndexer();
                }).GetAwaiter().GetResult();

                _searcher = new Searcher(index);
            }

        }

        private async Task<Indexer> LoadIndexer (){
            uint lastId = await InvertedIndexModel.GetLastId();
            Console.WriteLine("lastid" + lastId);
            return  new Indexer(lastId);
        }

        /// <summary>
        /// Method that handles search request, takes in query and run it against 
        /// </summary>
        /// <param name="parameters"> query string, autosuggestions bool </param>
        /// <returns> Response with list of matching documents </returns>
        [HttpGet]
        public ActionResult<List<Object>> Get([FromQuery]SearchQuery parameters)
        {
            if (ModelState.IsValid)
            {
                if (parameters.autocomplete)
                {

                    List<string> queryTokens = Stopwords.Clean(parameters.query);
                    
                    var data = searchifyContext.Suggestions.ToList().Where(s => Stopwords.compareQuery(queryTokens, s.query)).OrderBy(s => s.rank).ToList<Suggestions>();
                    IEnumerable<string> strippedData = data.Select(s => Helpers.MarkSuggestions(queryTokens, s.query)).Reverse().Take(5);
                    return Ok(new Response<List<string>>(strippedData.ToList(), "These are the generated queries"));
                }
                else
                {



                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    uint[] fileIdArray = Task.Run(async () => await _searcher.ExecuteQuery(parameters.query)).GetAwaiter().GetResult();
                    var fileIdsInts = fileIdArray.ToList().ConvertAll<int>(x => (int) x);
                    stopwatch.Stop();

                    List<Document> responsedata = searchifyContext.Documents.Where(a => fileIdsInts.Contains(a.id)).ToList();


                    //  note this should only be done if the query resulted in a fruitful response
                    var data = searchifyContext.Suggestions.Where(a => a.query.ToLower() == parameters.query.ToLower()).FirstOrDefault();
                    if (data == null)
                    {
                        Suggestions newSuggest = new Suggestions();
                        newSuggest.id = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                        newSuggest.query = parameters.query;
                        newSuggest.rank = 1;
                        searchifyContext.Suggestions.Add(newSuggest);
                    }
                    else
                    {
                        data.rank += 1;
                    }

                    searchifyContext.SaveChanges();
                    return Ok(new Response<List<Document>>(responsedata, "time taken:" + stopwatch.ElapsedMilliseconds));
                }


                    

            }
            
            return BadRequest();
        }
    }
}
