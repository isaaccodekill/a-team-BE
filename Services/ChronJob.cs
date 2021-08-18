using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Searchify.Domain.Models;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;


namespace Searchify.Services
{
    public class IndexCronJob: CronJobService
    {
        private readonly ILogger<IndexCronJob> _logger;
        private readonly SearchifyContext searchifyContext;

        public IndexCronJob(IScheduleConfig<IndexCronJob> config, ILogger<IndexCronJob> logger, IServiceScopeFactory factory)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            searchifyContext = factory.CreateScope().ServiceProvider.GetRequiredService<SearchifyContext>();
            _logger = logger;
        }


        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJob 1 is working.");
            var data = searchifyContext.Documents.Where(a => a.white_listed);
            if (data != null)
            {
                // pass white listed files to indexer
                Console.WriteLine("some files will be indexed");

            }


            return Task.CompletedTask;
        }

    }
}