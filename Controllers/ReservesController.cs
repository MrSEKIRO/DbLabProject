using DbLabProject.Context;
using DbLabProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DbLabProject.Controllers
{
    [Authorize]
    public class ReservesController : Controller
    {
        private readonly DatabaseContext _context;

        public ReservesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Reserves
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var databaseContext = _context.Reserves
                .Include(r => r.Food)
                .Include(r => r.Restaurant)
                .Include(r => r.Student)
                .Where(r => r.StudentId == userId);

            return View(await databaseContext.ToListAsync());
        }

        private int GetUserId()
        {
            return int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }

        // GET: Reserves/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var reserve = await _context.Reserves
                .Include(r => r.Food)
                .Include(r => r.Restaurant)
                .Include(r => r.Student)
                .Where(r => r.StudentId == GetUserId())
                .FirstOrDefaultAsync(m => m.Id == id);
            if(reserve == null)
            {
                return NotFound();
            }

            return View(reserve);
        }

        public IActionResult Search()
        {
            var userId = GetUserId();
            var foods = _context.Foods
                .Include(f => f.Restaurants)
                    .ThenInclude(r => r.AvailableStudnets)
                .Include(f => f.Reserves)
				    .ThenInclude(r => r.Student)
                //.Where(f => f.Restaurants.SelectMany(r => r.AvailableStudnets).Select(a => a.Id).Contains(userId))
                //.Where(f => f.Reserves.Select(r => r.StudentId).Contains(userId) == false)
                .ToList();
			
            var groups = foods.GroupBy(f => f.MealType + "/" + f.Date.ToString("dd-MM-yyyy"))
				//.Where(g => g.All(f => f.Reserves.All(r => r.StudentId != userId)))
                //.Select(g => g.Key)
                .ToList();


            var keys = new List<string>();
            foreach(var group in groups)
            {
                bool flag = false;
                foreach(var item in group)
                {
                    if(item.Reserves.Any(r => r.StudentId == userId))
                    {
                        flag = true;
                        break;
                    }
                }
				if(flag == false)
                {
                    keys.Add(group.Key);
                }
            }
			
            ViewBag.Groups = new SelectList(keys);

            return View();
        }

        // GET: Reserves/Create
        public IActionResult Create([Bind("Group")] SearchViewModel searchViewModel)
        {
            var userId = GetUserId();
            var foods = _context.Foods
                .Include(f => f.Restaurants)
                    .ThenInclude(r => r.AvailableStudnets)
                .Include(f => f.Reserves)
                //.Where(f => f.Restaurants.SelectMany(r => r.AvailableStudnets).Select(a => a.Id).Contains(userId))
                .Where(f => f.Reserves.Select(r => r.StudentId).Contains(userId) == false)
                .ToList();

            var group = foods.GroupBy(f => f.MealType + "/" + f.Date.ToString("dd-MM-yyyy"))
                .Where(g => g.Key == searchViewModel.Group)
                .FirstOrDefault();

            if(group == null)
            {
                return NotFound();
            }

            ViewData["FoodName"] = new SelectList(group.Select(g => g.Name).Distinct());
            ViewData["RestaurantName"] = new SelectList(group.SelectMany(f => f.Restaurants.Select(f => f.Name).Distinct()));
            return View();
        }

        // POST: Reserves/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RestaurantName", "FoodName")] ReserveViewModel reserveViewModel)
        {
            var userId = GetUserId();

            var restaurant = _context.Restaurants
                .Where(r => r.Name == reserveViewModel.RestaurantName)
                .First();

            var food = _context.Foods
                .Where(f => f.Name == reserveViewModel.FoodName)
                .First();

            var reserve = new Reserve()
            {
                Food = food,
                Restaurant = restaurant,
                StudentId = userId,
            };
            if(ModelState.IsValid)
            {
                _context.Add(reserve);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

			return RedirectToAction(nameof(Search));
		}

        // GET: Reserves/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if(id == null)
        //    {
        //        return NotFound();
        //    }

        //    var reserve = await _context.Reserves.FindAsync(id);
        //    if(reserve == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["FoodId"] = new SelectList(_context.Foods, "Id", "Id", reserve.FoodId);
        //    ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Id", reserve.RestaurantId);
        //    ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", reserve.StudentId);
        //    return View(reserve);
        //}

        //// POST: Reserves/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,RestaurantId,FoodId,StudentId")] Reserve reserve)
        //{
        //    if(id != reserve.Id)
        //    {
        //        return NotFound();
        //    }

        //    if(ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(reserve);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch(DbUpdateConcurrencyException)
        //        {
        //            if(!ReserveExists(reserve.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["FoodId"] = new SelectList(_context.Foods, "Id", "Id", reserve.FoodId);
        //    ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Id", reserve.RestaurantId);
        //    ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", reserve.StudentId);
        //    return View(reserve);
        //}

        // GET: Reserves/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var reserve = await _context.Reserves
                .Include(r => r.Food)
                .Include(r => r.Restaurant)
                .Include(r => r.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if(reserve == null)
            {
                return NotFound();
            }

            return View(reserve);
        }

        // POST: Reserves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserve = await _context.Reserves.FindAsync(id);
            _context.Reserves.Remove(reserve);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReserveExists(int id)
        {
            return _context.Reserves.Any(e => e.Id == id);
        }
    }
}
