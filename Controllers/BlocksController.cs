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
    public class BlocksController : Controller
    {
        private readonly DatabaseContext _context;

        public BlocksController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Blocks
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.Block.Include(b => b.Dormitory);
            return View(await databaseContext.ToListAsync());
        }

        // GET: Blocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var block = await _context.Block
                .Include(b => b.Dormitory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (block == null)
            {
                return NotFound();
            }

            return View(block);
        }

        // GET: Blocks/Create
        public IActionResult Create()
        {
            ViewData["DormitoryId"] = new SelectList(_context.Dormitories, "Id", "Id");
            return View();
        }

        // POST: Blocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,PricePerSemester,DormitoryId")] Block block)
        {
            if (ModelState.IsValid)
            {
                _context.Add(block);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DormitoryId"] = new SelectList(_context.Dormitories, "Id", "Id", block.DormitoryId);
            return View(block);
        }

        // GET: Blocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var block = await _context.Block.FindAsync(id);
            if (block == null)
            {
                return NotFound();
            }
            ViewData["DormitoryId"] = new SelectList(_context.Dormitories, "Id", "Id", block.DormitoryId);
            return View(block);
        }

        // POST: Blocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PricePerSemester,DormitoryId")] Block block)
        {
            if (id != block.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(block);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlockExists(block.Id))
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
            ViewData["DormitoryId"] = new SelectList(_context.Dormitories, "Id", "Id", block.DormitoryId);
            return View(block);
        }

        // GET: Blocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var block = await _context.Block
                .Include(b => b.Dormitory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (block == null)
            {
                return NotFound();
            }

            return View(block);
        }

        // POST: Blocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var block = await _context.Block.FindAsync(id);
            _context.Block.Remove(block);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlockExists(int id)
        {
            return _context.Block.Any(e => e.Id == id);
        }
    }
}
