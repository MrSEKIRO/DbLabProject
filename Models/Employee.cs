namespace DbLabProject.Models
{
	//public enum WorkingArea
	//{
	//	Restaurant,
	//	Dormitory,
	//}
	public class Employee
	{
		public int Id { get; set; }
		public string FullName { get; set; } = string.Empty;
		public string Role { get; set; } = string.Empty;
		//public WorkingArea WorkArea { get; set; }

		public Dormitory? Dormitory { get; set; }
		public int? DormitoryId { get; set; }

		public Restaurant? Restaurant { get; set; }
		public int? RestaurantId { get; set; }
	}
}
