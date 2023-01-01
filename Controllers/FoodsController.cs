using DbLabProject.Context;
using DbLabProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbLabProject.Controllers
{
    public class FoodsController : Controller
    {
        private readonly DatabaseContext _context;

        public FoodsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Foods
        public async Task<IActionResult> Index()
        {
            return View(await _context.Foods.ToListAsync());
        }

        // GET: Foods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var food = await _context.Foods
				.Include(f => f.Restaurants)
                .FirstOrDefaultAsync(m => m.Id == id);
            if(food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // GET: Foods/Create
        public IActionResult Create()
        {
            var weekdays = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>();
            ViewData["DayOfWeek"] = new SelectList(weekdays);

            var mealTypes = Enum.GetValues(typeof(MealType)).Cast<MealType>();
            ViewData["MealType"] = new SelectList(mealTypes);

            var food = new FoodViewModel()
            {
                Date = DateTime.Now,
                Restaurants = _context.Restaurants.Select(r => r.Name).ToList(),
            };

            return View(food);
        }

        // POST: Foods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Date,DayOfWeek,MealType,Price,Restaurants")] FoodViewModel foodViewModel)
        {
            if(ModelState.IsValid)
            {
                var food = new Food()
                {
                    Name = foodViewModel.Name,
                    Date = foodViewModel.Date,
                    DayOfWeek = foodViewModel.DayOfWeek,
                    MealType = foodViewModel.MealType,
                    Price = foodViewModel.Price,
                    Restaurants = _context.Restaurants
                        .Where(r => foodViewModel.Restaurants.Contains(r.Name))
                        .ToList(),
                };
                _context.Add(food);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var weekdays = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>();
            ViewData["DayOfWeek"] = new SelectList(weekdays);

            var mealTypes = Enum.GetValues(typeof(MealType)).Cast<MealType>();
            ViewData["MealType"] = new SelectList(mealTypes);
            return View(foodViewModel);
        }

        // GET: Foods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var food = await _context.Foods.FindAsync(id);
            if(food == null)
            {
                return NotFound();
            }

            var weekdays = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>();
            ViewData["DayOfWeek"] = new SelectList(weekdays);

            var mealTypes = Enum.GetValues(typeof(MealType)).Cast<MealType>();
            ViewData["MealType"] = new SelectList(mealTypes);

            var viewModelFood = new FoodViewModel()
            {
                Id = food.Id,
                Name = food.Name,
                Date = food.Date,
                DayOfWeek = food.DayOfWeek,
                MealType = food.MealType,
                Price = food.Price,
                Restaurants = _context.Restaurants.Select(r => r.Name).ToList(),
            };

            return View(viewModelFood);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Date,DayOfWeek,MealType,Price,Restaurants")] FoodViewModel foodViewModel)
        {
            if(id != foodViewModel.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    var food = _context.Foods
						.Include(f => f.Restaurants)
                        .FirstOrDefault(x => x.Id == foodViewModel.Id);
					
                    food.Name = foodViewModel.Name;
                    food.Date = foodViewModel.Date;
                    food.DayOfWeek = foodViewModel.DayOfWeek;
                    food.MealType = foodViewModel.MealType;
                    food.Price = foodViewModel.Price;
                    food.Restaurants = _context.Restaurants
                        .Where(r => foodViewModel.Restaurants.Contains(r.Name))
                        .ToList();
                    _context.Update(food);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!FoodExists(foodViewModel.Id))
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

            var weekdays = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>();
            ViewData["DayOfWeek"] = new SelectList(weekdays);

            var mealTypes = Enum.GetValues(typeof(MealType)).Cast<MealType>();
            ViewData["MealType"] = new SelectList(mealTypes);

            return View(foodViewModel);
        }

        // GET: Foods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var food = await _context.Foods
                .FirstOrDefaultAsync(m => m.Id == id);
            if(food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // POST: Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var food = await _context.Foods.FindAsync(id);
            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodExists(int id)
        {
            return _context.Foods.Any(e => e.Id == id);
        }
    }
}
