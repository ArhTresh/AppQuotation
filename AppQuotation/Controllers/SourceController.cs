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
    public class SourceController : Controller
    {
        private ApplicationContext db;
        public SourceController(ApplicationContext context)
        {
            db = context;
        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            return View(await db.Sources.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Source source)
        {
            db.Sources.Add(source);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Source source = await db.Sources.FirstOrDefaultAsync(p => p.Id == id);
                if (source != null)
                    return View(source);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Source source)
        {
            db.Sources.Update(source);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Source source = await db.Sources.FirstOrDefaultAsync(p => p.Id == id);
                if (source != null)
                    return View(source);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Source source = new Source { Id = id.Value };
                db.Entry(source).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
