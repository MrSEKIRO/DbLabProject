using DbLabProject.Models;
using Microsoft.EntityFrameworkCore;

namespace DbLabProject.Context
{
	public class DatabaseContext : DbContext
	{
		public DbSet<Restaurant> Restaurants { get; set; }
		public DbSet<Food> Foods { get; set; }
		public DbSet<Reserve> Reserves { get; set; }
		public DbSet<Student> Students { get; set; }
		public DbSet<Dormitory> Dormitories { get; set; }
		public DbSet<Employee> Employees { get; set; }
		public DbSet<Room> Rooms { get; set; }
		public DbSet<Tool> Tools { get; set; }


		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
		{ }


		public DbSet<DbLabProject.Models.Block> Block { get; set; }
	}
}
