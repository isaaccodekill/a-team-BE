using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Searchify.DynamoDb;
using System.Threading.Tasks;
using System;

namespace Searchify
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                await MainAsync(args);
            }).GetAwaiter().GetResult();
            CreateHostBuilder(args).Build().Run();
        }

        static async Task MainAsync(string[] args)
        {
            string env = Environment.GetEnvironmentVariable("NET_ENV") ?? "Development";
                if (env  == "Development")
                {
                    DbClient.CreateClient(true);
                }
                else
                {
                    DbClient.CreateClient(false);
                }
            await DbClient.CreateTables();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
