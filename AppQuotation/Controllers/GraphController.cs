using AppQuotation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppQuotation.Controllers
{
    public class GraphController : Controller
    {

        private ApplicationContext db;

        public GraphController(ApplicationContext context)
        {
            db = context;
        }

        [Authorize(Roles = "admin, user")]
        public IActionResult Chart()
        {
            var quotations = db.Quotations.Include(p => p.Company).Include(p => p.Source);

            //return View(await db.Quotations.ToListAsync());

            return View(quotations.ToList());
        }

        [Authorize(Roles = "admin, user")]
        public IActionResult selectCompany(long idCompany, long idSource)
        {
            var Quotations = db.Quotations.Include(p => p.Company);

            var lstModel = new List<SimpleReportViewModel>();


            foreach (Quotation q in Quotations)
            {
                if (q.CompanyId == idCompany)
                {
                    if (q.SourceId == idSource)
                    {
                        lstModel.Add(new SimpleReportViewModel
                        {
                            DimensionOne = q.Date.ToString(),
                            Quantity = q.Price
                        });
                    }
                }
                
            }

            var stacked = new StackedViewModel {
                StackedDimensionOne = Quotations.FirstOrDefault(x=> x.CompanyId == idCompany).Company.Name, 
                LstData = lstModel
            };

            return View(stacked);
        }
        


    }
}
