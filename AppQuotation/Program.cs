using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using AppQuotation.Job;
using Quartz.Impl;

namespace AppQuotation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices((hostContext, services) =>
            {
                services.AddQuartz(async q =>
                {

                    //var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");

                    //var appConfiguration = builder.Build();

                    //int period = int.Parse(appConfiguration["Quartz:QuotationJob"]);



                    StdSchedulerFactory factory = new StdSchedulerFactory();
                    IScheduler scheduler = await factory.GetScheduler();

                    // and start it off
                    await scheduler.Start();

                    // define the job and tie it to our HelloJob class
                    IJobDetail job = JobBuilder.Create<QuotationJob>()
                        .WithIdentity("job1", "group1")
                        .Build();

                    // Trigger the job to run now, and then repeat every 10 seconds
                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity("trigger1", "group1")
                        .StartNow()
                        .WithSimpleSchedule(x => x
                            .WithIntervalInSeconds(20)
                            .RepeatForever())
                        .Build();

                    // Tell quartz to schedule the job using our trigger
                    await scheduler.ScheduleJob(job, trigger);

                    // some sleep to show what's happening
                    //await Task.Delay(TimeSpan.FromSeconds(60));

                    // and last shut down the scheduler when you are ready to close your program
                    //await scheduler.Shutdown();
                });

                // ASP.NET Core hosting
                services.AddQuartzServer(options =>
                {
                    // when shutting down we want jobs to complete gracefully
                    options.WaitForJobsToComplete = true;
                });
            });
    }
}
