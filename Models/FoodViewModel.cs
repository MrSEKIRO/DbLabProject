using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace DbLabProject.Models
{
    public class FoodViewModel
    {
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;

		[DataType(DataType.Date)]
		public DateTime Date { get; set; }
		public DayOfWeek DayOfWeek { get; set; }
		public MealType MealType { get; set; }
		public int Price { get; set; }

		public List<string> Restaurants { get; set; } = new List<string>();
	}
}
