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


namespace Searchify.Controllers
{
    [ApiController]
    [Route("/api/search")]
    public class SearchController : ControllerBase
    {

        private readonly SearchifyContext searchifyContext;
        private readonly ILogger<SearchController> _logger;
        public SearchController(SearchifyContext context, ILogger<SearchController> logger)
        {
            searchifyContext = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]SearchQuery parameters)
        {
            if (ModelState.IsValid)
            {
                if (parameters.autocomplete)
                {

                    List<string> queryTokens =  Stopwords.Clean(parameters.query);

                    var data = searchifyContext.Suggestions.ToList().Where(s =>  Stopwords.compareQuery(queryTokens, s.query)).OrderBy(s => s.rank).ToList<Suggestions>();
                    IEnumerable<string> strippedData = data.Select(s => s.query).Reverse();
                    return Ok(new Response<IEnumerable<string>>(strippedData, "These are the generated queries"));
                }
                else
                {
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
                        data.rank += 1;
                    }

               searchifyContext.SaveChanges();
                return Ok(SearchService.DisplayResults(parameters.query.ToString()));

            }
            
            return BadRequest();
        }
    }
}
