using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Searchify.Domain.Models;
using Searchify.Domain.Utils;
using Searchify.Services;
using System.Text.Json;


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
        public ActionResult<Response<List<Document>>> Get(string id)
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
                data.id = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                searchifyContext.Add(data);
                searchifyContext.SaveChanges();
                return Ok(new Response<Document>(data, "document queued for indexing"));
            }
            return BadRequest();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
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
        public IActionResult update(string id, Document updatedDoc)
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
                    return Ok(new Response<Document>(data, "sucessfully queued doc for re-indexing"));
                }
            }
            return BadRequest();
        }
    }
}

