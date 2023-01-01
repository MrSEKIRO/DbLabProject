using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DbLabProject.Context;
using DbLabProject.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DbLabProject.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly DatabaseContext _context;

        public EmployeesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.Employees
                .Include(e => e.Dormitory)
                .Include(e => e.Restaurant);
            return View(await databaseContext.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Dormitory)
                .Include(e => e.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            var ls = new List<string>() { string.Empty };
            ls.AddRange(_context.Dormitories.Select(d => d.Id.ToString()));
			ViewData["DormitoryId"] = new SelectList(ls);
            
            var ls2 = new List<string>() { string.Empty };
            ls2.AddRange(_context.Restaurants.Select(d => d.Id.ToString()));
			ViewData["RestaurantId"] = new SelectList(ls2);
			
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Role,DormitoryId,RestaurantId")] Employee employee)
        {
            if(employee.RestaurantId !=null && employee.DormitoryId != null)
            {
                ModelState.AddModelError((Employee e) => e.RestaurantId, "Employee can work at one side only");
                ModelState.AddModelError((Employee e) => e.DormitoryId, "Employee can work at one side only");
            }
			
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
			var ls = new List<string>() { string.Empty };
			ls.AddRange(_context.Dormitories.Select(d => d.Id.ToString()));
			ViewData["DormitoryId"] = new SelectList(ls);

			var ls2 = new List<string>() { string.Empty };
			ls2.AddRange(_context.Restaurants.Select(d => d.Id.ToString()));
			ViewData["RestaurantId"] = new SelectList(ls2);
			return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
			var ls = new List<string>() { string.Empty };
			ls.AddRange(_context.Dormitories.Select(d => d.Id.ToString()));
			ViewData["DormitoryId"] = new SelectList(ls);

			var ls2 = new List<string>() { string.Empty };
			ls2.AddRange(_context.Restaurants.Select(d => d.Id.ToString()));
			ViewData["RestaurantId"] = new SelectList(ls2);
			return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Role,DormitoryId,RestaurantId")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }
			
			if(employee.RestaurantId != null && employee.DormitoryId != null)
			{
				ModelState.AddModelError((Employee e) => e.RestaurantId, "Employee can work at one side only");
				ModelState.AddModelError((Employee e) => e.DormitoryId, "Employee can work at one side only");
			}
			if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
			var ls = new List<string>() { string.Empty };
			ls.AddRange(_context.Dormitories.Select(d => d.Id.ToString()));
			ViewData["DormitoryId"] = new SelectList(ls);

			var ls2 = new List<string>() { string.Empty };
			ls2.AddRange(_context.Restaurants.Select(d => d.Id.ToString()));
			ViewData["RestaurantId"] = new SelectList(ls2);
			return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Dormitory)
                .Include(e => e.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
