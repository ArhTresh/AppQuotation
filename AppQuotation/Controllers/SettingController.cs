using AppQuotation.Job;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppQuotation.Controllers
{
    public class SettingController : Controller
    {
        // GET: SettingController

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View();
        }
        
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> IndexAsync(int time)
        {

           
            string json = System.IO.File.ReadAllText("appsettings.json");
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            jsonObj["Quartz"]["QuotationJob"] = time.ToString();
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText("appsettings.json", output);

            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();

            await scheduler.Shutdown();

            IScheduler scheduler2 = await factory.GetScheduler();

            // and start it off
            await scheduler2.Start();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<QuotationJob>()
                .WithIdentity("job1", "group1")
                .Build();

            // Trigger the job to run now, and then repeat every 10 seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(time)
                    .RepeatForever())
                .Build();

            // Tell quartz to schedule the job using our trigger
            await scheduler2.ScheduleJob(job, trigger);

            return View();

        }
    }
}
