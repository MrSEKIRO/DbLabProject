using DbLabProject.Context;
using DbLabProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbLabProject.Controllers
{
    public class ToolsController : Controller
    {
        private readonly DatabaseContext _context;

        public ToolsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Tools
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.Tools
                .Include(t => t.Room);
            return View(await databaseContext.ToListAsync());
        }

        // GET: Tools/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var tool = await _context.Tools
                .Include(t => t.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if(tool == null)
            {
                return NotFound();
            }

            return View(tool);
        }

        // GET: Tools/Create
        public IActionResult Create()
        {
            var rooms = new List<string>() { string.Empty };
            var dbRooms = _context.Rooms
                .Select(r => r.Id.ToString())
                .ToList();
            rooms.AddRange(dbRooms);

            ViewData["RoomId"] = new SelectList(rooms);

            return View();
        }

        // POST: Tools/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IsInDepot,RoomId")] Tool tool)
        {
            if(tool.IsInDepot == false && tool.RoomId == null
                || tool.IsInDepot==true && tool.RoomId != null)
            {
                ModelState.AddModelError((Tool t) => t.RoomId, "The tool should be in room or in depot");
            }

            if(ModelState.IsValid)
            {
                _context.Add(tool);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

			var rooms = new List<string>() { string.Empty };
			var dbRooms = _context.Rooms
				.Select(r => r.Id.ToString())
				.ToList();
			rooms.AddRange(dbRooms);
			ViewData["RoomId"] = new SelectList(rooms);

			return View(tool);
        }

        // GET: Tools/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var tool = await _context.Tools.FindAsync(id);
            if(tool == null)
            {
                return NotFound();
            }

			var rooms = new List<string>() { string.Empty };
			var dbRooms = _context.Rooms
				.Select(r => r.Id.ToString())
				.ToList();
			rooms.AddRange(dbRooms);
			ViewData["RoomId"] = new SelectList(rooms);

			return View(tool);
        }

        // POST: Tools/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IsInDepot,RoomId")] Tool tool)
        {
            if(id != tool.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(tool);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!ToolExists(tool.Id))
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

			var rooms = new List<string>() { string.Empty };
			var dbRooms = _context.Rooms
				.Select(r => r.Id.ToString())
				.ToList();
			rooms.AddRange(dbRooms);
			ViewData["RoomId"] = new SelectList(rooms);

			return View(tool);
        }

        // GET: Tools/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var tool = await _context.Tools
                .Include(t => t.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if(tool == null)
            {
                return NotFound();
            }

            return View(tool);
        }

        // POST: Tools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tool = await _context.Tools.FindAsync(id);
            _context.Tools.Remove(tool);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToolExists(int id)
        {
            return _context.Tools.Any(e => e.Id == id);
        }
    }
}
