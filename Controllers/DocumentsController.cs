using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Searchify.Domain.Models;
using Searchify.Domain.Utils;
using Searchify.Services;


namespace Searchify.Controllers
{
    [ApiController]
    [Route("/api/docs")]
    public class DocumentController : ControllerBase
    {

        private readonly SearchifyContext searchifyContext;

        private readonly ILogger<DocumentController> _logger;

        public DocumentController(SearchifyContext context,  ILogger<DocumentController> logger)
        {
            searchifyContext = context;
            _logger = logger;
        }

        [HttpGet]
        public  ActionResult<Response<List<Document>>> GetAll()
        {

            var data = searchifyContext.Documents.ToList();
            if (data == null) return NotFound();
            return Ok(new Response<List<Document>>(data, ""));
        }



        [HttpGet("{id}")]
        public ActionResult<Response<List<Document>>> Get(int id)
        {
            var data = searchifyContext.Documents.Where(a => a.id == id).FirstOrDefault();
            if (data == null) return NotFound();
            else
            {
                return Ok(new Response<Document>(data, "success"));
            }
        }



        [HttpPost]
        public ActionResult<Response<Document>> Post(Document data)
        {
           
            if (ModelState.IsValid)
            {
                data.white_listed = true;
                data.black_listed = false;
                searchifyContext.Add(data);
                searchifyContext.SaveChanges();

                var successfulIndexing = IndexingService.CallIndexer(data);
                if (successfulIndexing)
                {
                    return Ok(new Response<Document>(data, "document queued for indexing"));
                }
                else
                {
                    searchifyContext.Remove(data);
                    return BadRequest(new Response<Object>(new Object {}, "Failed to index please try again later"));
                }
                
            }
            return BadRequest();

        }

        [HttpPost("/api/search/batch-upload")]
        public ActionResult<Response<Document>> Post(List<Document> data)
        {
            if (ModelState.IsValid)
            {
                for(var i = 0; i < data.Count(); i++)
                {
                    data[i].black_listed = false;
                }
                searchifyContext.AddRange(data);
                searchifyContext.SaveChanges();
                var successfulIndexing = IndexingService.CallIndexer(data);
                if (successfulIndexing)
                {
                    return Ok(new Response<List<Document>>(data, "documents queued for re-indexing"));
                }
                else
                {
                    searchifyContext.RemoveRange(data);
                    return BadRequest(new Response<Object>(new Object { }, "Failed to index please try again later"));
                }
            }
            return BadRequest();
        }




        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            if (ModelState.IsValid)
            {
                var data = searchifyContext.Documents.Where(a => a.id == id).FirstOrDefault();
                if (data == null) return NotFound();
                else {
                    data.white_listed = false;
                    data.black_listed = true;
                    searchifyContext.SaveChanges();
                    return Ok( new Response<Document>(data, "sucessfully removed document from index"));
                }
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public IActionResult update(int id, Document updatedDoc)
        {

            if (ModelState.IsValid)
            {
                var data = searchifyContext.Documents.Where(a => a.id == id).FirstOrDefault();
                if (data == null) return NotFound();
                else
                {
                    data.url = updatedDoc.url;
                    data.name = updatedDoc.name;
                    data.preview_text = updatedDoc.preview_text;
                    data.white_listed = false;
                    data.black_listed = true;
                    searchifyContext.SaveChanges();

                    var successfulIndexing = IndexingService.CallIndexer(data);
                    if (successfulIndexing)
                    {
                        return Ok(new Response<Document>(data, "document queued for re-indexing"));
                    }
                    else
                    {
                        searchifyContext.Remove(data);
                        return BadRequest(new Response<Object>(new Object { }, "Failed to index please try again later"));
                    }

                }
            }
            return BadRequest();
        }
    }
}

