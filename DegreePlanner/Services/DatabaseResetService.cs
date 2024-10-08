using DegreePlanner.Data;
using Microsoft.EntityFrameworkCore;

namespace DegreePlanner.Services
{
	public class DatabaseResetService(DatabaseContext database)
	{
		public void ResetDatabase()
		{
			// Clear all existing data from all tables
			database.DegreeSubjects.ExecuteDelete();
			database.MajorSubjects.ExecuteDelete();
			database.UserSubjects.ExecuteDelete();
			database.Users.ExecuteDelete();
			database.Majors.ExecuteDelete();
			database.Degrees.ExecuteDelete();
			database.Subjects.ExecuteDelete();

			Subject prog1 = new()
			{
				Name = "Programming 1"
			};
			Subject prog2 = new()
			{
				Name = "Programming 2",
				Prerequisites = [prog1]
			};
			Subject dotnet = new()
			{
				Name = "Application Development with .NET",
				Prerequisites = [prog2]
			};
			Subject linearAlgebra = new()
			{
				Name = "Linear Algebra"
			};
			Subject probability = new()
			{
				Name = "Probability and Random Variables"
			};
			Subject brm = new()
			{
				Name = "Business Requirements Modelling"
			};
			List<Subject> subjects = [prog1, prog2, dotnet, linearAlgebra, probability, brm];


			List<Major> majors = [];
			majors.Add(new()
			{
				Name = "Enterprise Software Development",
				Credits = 48,
				Subjects = [prog1, dotnet]
			});
			majors.Add(new()
			{
				Name = "Mathematical Analysis",
				Credits = 48,
				Subjects = [linearAlgebra, probability]
			});

			Degree compsci = new()
			{
				Name = "Computer Science",
				CoreCredits = 48,
				ElectiveCredits = 48,
				Majors = [majors[0], majors[1]],
				Subjects = [prog1]
			};
			Degree bit = new()
			{
				Name = "Bachelor of Information Technology",
				CoreCredits = 48,
				ElectiveCredits = 48,
				Majors = [majors[0]],
				Subjects = [brm]
			};
			List<Degree> degrees = [compsci, bit];

			List<User> users = [];
			users.Add(new()
			{
				Name = "Tai",
				Password = "test",
				Role = UserRole.Student,
				Degree = bit,
				Major = bit.Majors[0],
			});
			users.Add(new()
			{
				Name = "David",
				Password = "test",
				Role = UserRole.Staff,
			});
			users.Add(new()
			{
				Name = "Admin",
				Password = "test",
				Role = UserRole.Admin,
			});

			database.Subjects.AddRange(subjects);
			database.Majors.AddRange(majors);
			database.Degrees.AddRange(degrees);
			database.Users.AddRange(users);

			database.SaveChanges();

			var degreeSubjects = database.DegreeSubjects.ToList();
			degreeSubjects[0].Type = DegreeSubjectType.Core;
			degreeSubjects[1].Type = DegreeSubjectType.Elective;
			database.SaveChanges();
		}
	}
}
