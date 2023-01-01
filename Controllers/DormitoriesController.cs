using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DbLabProject.Context;
using DbLabProject.Models;

namespace DbLabProject.Controllers
{
    public class DormitoriesController : Controller
    {
        private readonly DatabaseContext _context;

        public DormitoriesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Dormitories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Dormitories.ToListAsync());
        }

        // GET: Dormitories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dormitory = await _context.Dormitories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dormitory == null)
            {
                return NotFound();
            }

            return View(dormitory);
        }

        // GET: Dormitories/Create
        public IActionResult Create()
        {
			SelectList degrees = new SelectList(new List<string>
			{
				DegreeType.Bachelor.ToString(),
				DegreeType.Master.ToString(),
				DegreeType.PhD.ToString(),
			});
			ViewBag.Degrees = degrees;
			return View();
        }

        // POST: Dormitories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Location,IsAvailable,Capacity,AvailablePlaces,Degree")] Dormitory dormitory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dormitory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

			SelectList degrees = new SelectList(new List<string>
			{
				DegreeType.Bachelor.ToString(),
				DegreeType.Master.ToString(),
				DegreeType.PhD.ToString(),
			});
			ViewBag.Degrees = degrees;

			return View(dormitory);
        }

        // GET: Dormitories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dormitory = await _context.Dormitories.FindAsync(id);
            if (dormitory == null)
            {
                return NotFound();
            }

			SelectList degrees = new SelectList(new List<string>
			{
				DegreeType.Bachelor.ToString(),
				DegreeType.Master.ToString(),
				DegreeType.PhD.ToString(),
			});
			ViewBag.Degrees = degrees;

			return View(dormitory);
        }

        // POST: Dormitories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location,IsAvailable,Capacity,AvailablePlaces,Degree")] Dormitory dormitory)
        {
            if (id != dormitory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dormitory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DormitoryExists(dormitory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

			SelectList degrees = new SelectList(new List<string>
			{
				DegreeType.Bachelor.ToString(),
				DegreeType.Master.ToString(),
				DegreeType.PhD.ToString(),
			});
			ViewBag.Degrees = degrees;
			return View(dormitory);
        }

        // GET: Dormitories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dormitory = await _context.Dormitories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dormitory == null)
            {
                return NotFound();
            }

            return View(dormitory);
        }

        // POST: Dormitories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dormitory = await _context.Dormitories.FindAsync(id);
            _context.Dormitories.Remove(dormitory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DormitoryExists(int id)
        {
            return _context.Dormitories.Any(e => e.Id == id);
        }
    }
}
