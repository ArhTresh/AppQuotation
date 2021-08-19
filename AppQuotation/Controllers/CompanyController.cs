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
    public class CompanyController : Controller
    {
        private ApplicationContext db;
        public CompanyController(ApplicationContext context)
        {
            db = context;
        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            return View(await db.Companies.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Company company)
        {
            db.Companies.Add(company);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Company company = await db.Companies.FirstOrDefaultAsync(p => p.Id == id);
                if (company != null)
                    return View(company);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Company company)
        {
            db.Companies.Update(company);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Company company = await db.Companies.FirstOrDefaultAsync(p => p.Id == id);
                if (company != null)
                    return View(company);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Company company = new Company { Id = id.Value };
                db.Entry(company).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
