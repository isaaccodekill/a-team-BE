using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Searchify.Domain.Models;

namespace Searchify.Services
{
    public class IndexCronJob: CronJobService
    {
        private readonly string filepath = "whitelist.json";
        private readonly ILogger<IndexCronJob> _logger;

        public IndexCronJob(IScheduleConfig<IndexCronJob> config, ILogger<IndexCronJob> logger)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
        }


        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJob 1 is working.");
                if (File.Exists(filepath))
            {
                using (StreamReader r = new StreamReader(filepath))
                {
                    var json = r.ReadToEnd();
                    if (json.Length > 0)
                    {
                        var newFiles = JsonSerializer.Deserialize<IEnumerable<Document>>(json);
                        Console.WriteLine("indexing the new files. " + json);
                    }

                }
                File.WriteAllText(filepath, "");

            }


            return Task.CompletedTask;
        }

    }
}