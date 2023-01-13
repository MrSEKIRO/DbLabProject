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
    public class StudentsController : Controller
    {
        private readonly DatabaseContext _context;

        public StudentsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.Students.Include(s => s.Room);
            return View(await databaseContext.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if(student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            SelectList degrees = new SelectList(new List<string>
            {
                DegreeType.Bachelor.ToString(),
                DegreeType.Master.ToString(),
                DegreeType.PhD.ToString(),
            });
            ViewBag.Degrees = degrees;

            //var rooms = _context.Rooms
            //    .Include(r => r.Students)
            //    .Where(r => r.Students.Count < r.Capacity)
            //    .ToList();

            //ViewData["RoomId"] = new SelectList(rooms, "Id", "Id");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,SSN,Degree,CanUseDormitory,Phone")] Student student)
        {
            bool isNameUnique = IsNameUnique(student);
            bool isSSNUnique = IsSSNUnique(student);

            if(isNameUnique == false)
            {
                ModelState.AddModelError((Student s) => s.FullName, "Name is redundant");
            }
            if(isSSNUnique == false)
            {
				ModelState.AddModelError((Student s) => s.SSN, "SSN is redundant");
			}

			if(ModelState.IsValid)
            {
                var room = _context.Rooms
                    .Where(r => r.Id == student.RoomId)
                    .FirstOrDefault();
                student.Room = room;
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

			//var rooms = _context.Rooms
			//	.Include(r => r.Students)
			//	.Where(r => r.Students.Count < r.Capacity)
			//	.ToList();

			//ViewData["RoomId"] = new SelectList(rooms, "Id", "Id");
			return View(student);
        }

        private bool IsNameUnique(Student student)
        {
            return !_context.Students.Any(s => s.FullName == student.FullName);
        }
        private bool IsSSNUnique(Student student)
        {
            return !_context.Students.Any(s => s.SSN == student.SSN);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
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

            var student = await _context.Students.FindAsync(id);
            if(student == null)
            {
                return NotFound();
            }

			var rooms = _context.Rooms
			   .Include(r => r.Students)
			   .Where(r => r.Students.Count < r.Capacity)
			   .ToList();

			ViewData["RoomId"] = new SelectList(rooms, "Id", "Id");

			return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,SSN,Degree,CanUseDormitory,Phone,RoomId")] Student student)
        {
            if(id != student.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    if(student.CanUseDormitory == false)
                    {
                        student.RoomId = null;
                    }
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!StudentExists(student.Id))
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

			var rooms = _context.Rooms
				.Include(r => r.Students)
				.Where(r => r.Students.Count < r.Capacity)
				.ToList();

			ViewData["RoomId"] = new SelectList(rooms, "Id", "Id");
			return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if(student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
