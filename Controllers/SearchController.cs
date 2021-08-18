using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Searchify.Domain.Models;
using Searchify.Domain.Utils;
using Searchify.Services;

namespace Searchify.Controllers
{
    [ApiController]
    [Route("/api/search")]
    public class SearchController : ControllerBase
    {

        private readonly ILogger<SearchController> _logger;

        public SearchController(ILogger<SearchController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]SearchQuery parameters)
        {
            if (ModelState.IsValid)
            {

                if (parameters.autocomplete)
                {
                    return Ok(GenerateQueriesService.DisplayResults(parameters.query.ToString()));
                }
                else
                {
                    return Ok(SearchService.DisplayResults(parameters.query.ToString()));
                }
            }
            return BadRequest();
        }
    }
}
