using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Searchify.Controllers;
using Searchify.Domain.Models;
using Searchify.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;


namespace SearchTest
{
    public class UnitTest2
    {
        [Fact]
        public void GetDocs_Returns_Correct_No_of_docs()
        {

            // arrange

            var options = new DbContextOptionsBuilder<SearchifyContext>()
            .UseInMemoryDatabase(databaseName: "suggestions").Options;

            using (var context = new SearchifyContext(options))
            {
                context.Suggestions.Add(new Suggestions { id = "1", query = "This is a test with fashina", rank  = 2 });
                context.Suggestions.Add(new Suggestions { id = "3", query = "textbook recommendation fashina", rank = 0 });
                context.Suggestions.Add(new Suggestions { id = "2", query = "random text", rank = 0 });
                context.SaveChanges();

                // Create a DocumentController and invoke the Index action
                var controller = new SearchController(context, true);
                var actionResult = controller.Get(new SearchQuery { query =  "recommendation fashina", autocomplete = true });
                var result = actionResult.Result as OkObjectResult;
                var returnedSuggestion = result.Value as Response<List<string>>;
                Assert.Equal(200, result.StatusCode);
                Assert.Equal("textbook <mark> recommendation </mark> <mark> fashina </mark>", returnedSuggestion.data[0]);
                context.Database.EnsureDeleted();

            }


        }

    }
}
