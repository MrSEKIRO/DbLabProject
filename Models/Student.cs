using System.Collections.Generic;

namespace DbLabProject.Models
{
	public class Student
	{
		public int Id { get; set; }
		public string FullName { get; set; } = string.Empty;
		public int SSN { get; set; }
		public DegreeType Degree { get; set; }
		public bool CanUseDormitory { get; set; }

		public string Phone { get; set; } = "No phone number";
		//public string Email { get; set; }
		//public string Address { get; set; }
		//public string City { get; set; }

		public Room? Room { get; set; }
		public int? RoomId { get; set; }
	}

	public class Dormitory
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Location { get; set; } = string.Empty;
		public bool IsAvailable { get; set; } = true;
		public DegreeType Degree { get; set; }

		public List<Block> Blocks { get; set; } = new List<Block>();
	}

	public class Block
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public int PricePerSemester { get; set; }

		public Dormitory? Dormitory { get; set; }
		public int DormitoryId { get; set; }

		public List<Room> Rooms { get; set; } = new List<Room>();
	}

	public class Room
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public int Capacity { get; set; }
		public Block? Block { get; set; }
		public int BlockId { get; set; }

		public List<Student> Students { get; set; } = new List<Student>();
		public List<Tool> Tools { get; set; } = new List<Tool>();
	}

	public class Tool
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public bool IsInDepot { get; set; } = false;

		public Room? Room { get; set; }
		public int? RoomId { get; set; }
	}
}
