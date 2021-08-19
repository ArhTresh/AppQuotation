using AppQuotation.Models;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppQuotation.Job
{

    [DisallowConcurrentExecution]
    public class QuotationJob : IJob
    {
        //private readonly ILogger<QuotationJob> _logger;

        //public QuotationJob(ILogger<QuotationJob> logger)
        //{
        //    _logger = logger;
        //}

        public async Task Execute(IJobExecutionContext context)
        {

            //_logger.LogInformation("Hello world!");
            QuotationUpdate quotationUpdate = new QuotationUpdate();
            await quotationUpdate.UpdateAsync();
            return;
        }
    }

   

}
