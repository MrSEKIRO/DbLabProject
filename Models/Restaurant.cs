using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbLabProject.Models
{
	public enum MealType
	{
		Lunch,
		Dinner,
	}
	public class Restaurant
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public bool IsAvailable { get; set; } = true;
		public string Location { get; set; } = string.Empty;

		public List<Food> Foods { get; set; } = new List<Food>();
		public List<Reserve> Reserves { get; set; } = new List<Reserve>();
		public List<Student> AvailableStudnets { get; set; } = new List<Student>();
		public List<Employee> Employees { get; set; } = new List<Employee>();
	}

	public class Food
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		
		[DataType(DataType.Date)]
		public DateTime Date { get; set; }
		public DayOfWeek DayOfWeek { get; set; }
		public MealType MealType { get; set; }
		public int Price { get; set; }

		public List<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
		public List<Reserve> Reserves { get; set; } = new List<Reserve>();
	}

	public class Reserve
	{
		public int Id { get; set; }

		public Restaurant? Restaurant { get; set; }
		public int RestaurantId { get; set; }

		public Food? Food { get; set; }
		public int FoodId { get; set; }

		public Student? Student { get; set; }
		public int StudentId { get; set; }
	}
}
