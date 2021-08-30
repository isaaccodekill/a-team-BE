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

    /// <summary>
    /// Document controller class 
    /// </summary>

    [ApiController]
    [Route("/api/docs")]
    public class DocumentController : ControllerBase
    {

        private readonly SearchifyContext searchifyContext;


        public DocumentController(SearchifyContext context)
        {
            searchifyContext = context;
        }

        /// <summary>
        /// Handles Api request to get all documents
        /// </summary>
        /// <returns>
        ///   A Response instance Containing a List of Documents <see cref="Document"/> and <see cref="Response{T}"/>
        /// </returns>

        [HttpGet]
        public  ActionResult<Response<List<Document>>> GetAll()
        {

            var data = searchifyContext.Documents.ToList();
            if (data == null) return NotFound();
            return Ok(new Response<List<Document>>(data, ""));
        }


        /// <summary>
        /// Controler method that returns information about a single book
        /// </summary>
        /// <param name="id"> Book id passed from the request url </param>
        /// <returns>
        ///   A Response instance Containing a Document <see cref="Document"/> and <see cref="Response{T}"/>
        /// </returns>

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


        /// <summary>
        /// Controler method that created a Document
        /// </summary>
        /// <param name="data"> JSon data to create the book from </param>
        /// <returns>
        ///   A Response instance Containing created Document <see cref="Document"/> and <see cref="Response{T}"/>
        /// </returns>

        [HttpPost]
        public ActionResult<Response<Document>> Post(Document data)
        {
           
            if (ModelState.IsValid)
            {
                data.white_listed = true;
                data.black_listed = false;
                Console.WriteLine(data);
                searchifyContext.Add(data);
                searchifyContext.SaveChanges();

                bool successfulIndexing = Task.Run(async () => await IndexingService.Index(data, "")).GetAwaiter().GetResult();
                
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
        /// <summary>
        /// Controler method that allows mutliple document update
        /// </summary>
        /// <param name="data"> List of Documents </param>
        /// <returns>
        ///   A Response instance Containing a Documents <see cref="Document"/> and <see cref="Response{T}"/>
        /// </returns>
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
                
                var successfulIndexing = Task.Run(async () =>  await IndexingService.IndexMany(data)).GetAwaiter().GetResult();
                
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


        /// <summary>
        /// Controler method that deletes a book with id passed in request url
        /// </summary>
        /// <param name="id"> Id of the document to be deleted </param>
        /// <returns>
        ///   A Response instance containing deleted document <see cref="Document"/> and <see cref="Response{T}"/>
        /// </returns>

        [HttpDelete("{id}")]
        public ActionResult<Response<Document>> Delete(int id)
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


        /// <summary>
        /// Controler method that updates a book with id passed in request url
        /// </summary>
        /// <param name="id"> Id of the document to be updates </param>
        /// <returns>
        ///   A Response instance containing updated document <see cref="Document"/> and <see cref="Response{T}"/>
        /// </returns>


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

                    bool successfulIndexing = Task.Run(async () => await IndexingService.Index(data, "")).GetAwaiter().GetResult();

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

