using AppQuotation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using Microsoft.AspNetCore.Authorization;

namespace AppQuotation.Controllers
{
    public class QuotationController : Controller
    {
        private ApplicationContext db;
        public QuotationController(ApplicationContext context)
        {
            db = context;
        }
        
        [Authorize(Roles = "admin, user")]
        public IActionResult Index()
        {

            var quotations = db.Quotations.Include(p => p.Company).Include(p => p.Source);

            return View(quotations.ToList());
        }


                
    }
}
