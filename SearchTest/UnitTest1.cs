using System;
using Xunit;
using Searchify.Controllers;
using Searchify.Domain.Models;
using Searchify.Domain.Utils;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SearchTest
{
    public class DocumentControllerTest
{

    [Fact]
    public void  GetDocs_Returns_Correct_No_of_docs()
    {

            // arrange

            var options = new DbContextOptionsBuilder<SearchifyContext>()
            .UseInMemoryDatabase(databaseName: "DocumentsTest1").Options;

            using (var context = new SearchifyContext(options))
            {
                context.Documents.Add(new Document { id = 10, url = "testpdf.com", name = "book 1", preview_text = "book 1 preview" });
                context.Documents.Add(new Document { id = 11, url = "testpdf2.com", name = "book 2", preview_text = "book 2 preview" });
                context.Documents.Add(new Document { id = 12, url = "testpdf3.com", name = "book 3", preview_text = "book 3 preview" });
                context.SaveChanges();

                // Create a DocumentController and invoke the Index action
                var controller = new DocumentController(context);
                var actionResult = controller.GetAll();

                var result = actionResult.Result as OkObjectResult;
                var returnedBooks = result.Value as Response<List<Document>>;
                Assert.Equal(200, result.StatusCode);
                Assert.Equal(3, returnedBooks.data.Count);
                context.Database.EnsureDeleted();

            }


        }

        [Fact]
        public void GetDoc_Returns_Correct_name_of_doc()
        {

            // arrange

            var options = new DbContextOptionsBuilder<SearchifyContext>()
            .UseInMemoryDatabase(databaseName: "DocumentsTest1").Options;

            using (var context = new SearchifyContext(options))
            {


                context.Documents.Add(new Document { id = 29, url = "testpdf.com", name = "book 1", preview_text = "book 1 preview" });
                context.SaveChanges();

                // Create a DocumentController and invoke the Index action
                var controller = new DocumentController(context);
                
                var actionResult = controller.Get(29);
                var result = actionResult.Result as OkObjectResult;
                var returnedBooks = result.Value;
                var returnedDocs = returnedBooks as Response<Document>;
                Assert.Equal(200, result.StatusCode);
                Assert.Equal("book 1", returnedDocs.data.name);
                context.Database.EnsureDeleted();

            }

        }


        [Fact]
        public void DeleteDoc_Test_docDelete()
        {

            // arrange

            var options = new DbContextOptionsBuilder<SearchifyContext>()
            .UseInMemoryDatabase(databaseName: "DocumentsTest2").Options;

            using (var context = new SearchifyContext(options))
            {

                context.Documents.Add(new Document { id = 90, url = "testpdf.com", name = "book 1", preview_text = "book 1 preview" });
                context.Documents.Add(new Document { id = 92, url = "testpdf2.com", name = "book 2", preview_text = "book 2 preview" });
                context.Documents.Add(new Document { id = 24, url = "testpdf3.com", name = "book 3", preview_text = "book 3 preview" });
                context.SaveChanges();

                var controller = new DocumentController(context);

                var actionResult = controller.Delete(24);

                var result = actionResult.Result as OkObjectResult;
                var returnedBooks = result.Value;
                var returnedDocs = returnedBooks as Response<Document>;
                Assert.Equal(200, result.StatusCode);
                Assert.Equal(24, returnedDocs.data.id);
                context.Database.EnsureDeleted();

            }

        }
    }
}

