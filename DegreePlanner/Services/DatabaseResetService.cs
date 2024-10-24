using DegreePlanner.Data;
using Microsoft.EntityFrameworkCore;

namespace DegreePlanner.Services;

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

		database.Database.ExecuteSql($"DBCC CHECKIDENT('Users', RESEED, 10000)");

		// Create subjects
		Subject prog1 = new()
		{
			Name = "Programming 1",
			SubjectCode = 41039
		};
		Subject prog2 = new()
		{
			Name = "Programming 2",
			SubjectCode = 48024,
			Prerequisites = [prog1]
		};
		Subject dotnet = new()
		{
			Name = "Application Development with .NET",
			SubjectCode = 31927,
			Prerequisites = [prog2]
		};
		Subject linearAlgebra = new()
		{
			Name = "Linear Algebra",
			SubjectCode = 37233
		};
		Subject probability = new()
		{
			Name = "Probability and Random Variables",
			SubjectCode = 37161
		};
		Subject brm = new()
		{
			Name = "Business Requirements Modelling",
			SubjectCode = 31269
		};
		Subject citp = new()
		{
			Name = "Communication for IT Professionals",
			SubjectCode = 31265
		};
		Subject webSys = new()
		{
			Name = "Web Systems",
			SubjectCode = 31268
		};
		Subject dbFun = new()
		{
			Name = "Database Fundamentals",
			SubjectCode = 31271
		};
		Subject ios = new()
		{
			Name = "Application Development in the iOS Environment",
			SubjectCode = 41889,
			Prerequisites = [prog1]
		};
		Subject math1 = new()
		{
			Name = "Mathematics 1",
			SubjectCode = 33130
		};
		List<Subject> subjects = [prog1, prog2, dotnet, linearAlgebra, probability, brm, citp, webSys, dbFun, ios, math1];

		// Create majors containing these subjects
		List<Major> majors =
		[
			new Major
			{
				Name = "Enterprise Software Development",
				Credits = 48,
				Subjects = [prog2, dotnet, ios]
			},

			new Major
			{
				Name = "Mathematical Analysis",
				Credits = 48,
				Subjects = [linearAlgebra, probability]
			}
		];

		// Create degrees containing majors and subjects
		Degree compsci = new()
		{
			Name = "Bachelor of Computing Science",
			CoreCredits = 48,
			ElectiveCredits = 48,
			Majors = [majors[0], majors[1]],
			Subjects = [prog1, citp, webSys, math1]
		};
		Degree bit = new()
		{
			Name = "Bachelor of Information Technology",
			CoreCredits = 48,
			ElectiveCredits = 48,
			Majors = [majors[0]],
			Subjects = [brm, citp, webSys, dbFun, prog1]
		};
		List<Degree> degrees = [compsci, bit];

		// Create sample users
		List<User> users =
		[
			new User
			{
				Name = "Tai",
				Password = "test",
				Role = UserRole.Student,
				Degree = bit,
				Major = bit.Majors[0]
			},

			new User
			{
				Name = "David",
				Password = "test",
				Role = UserRole.Staff
			},

			new User
			{
				Name = "Avinash",
				Password = "test",
				Role = UserRole.Staff
			},

			new User
			{
				Name = "Admin",
				Password = "test",
				Role = UserRole.Admin
			}
		];

		// Add created objects to the database
		database.Subjects.AddRange(subjects);
		database.Majors.AddRange(majors);
		database.Degrees.AddRange(degrees);
		database.Users.AddRange(users);

		database.SaveChanges();

		// Update the types of subjects in the associative degree subject table
		var degreeSubjects = database.DegreeSubjects.ToList();
		degreeSubjects[0].Type = DegreeSubjectType.Core;
		degreeSubjects[1].Type = DegreeSubjectType.Elective;
		database.SaveChanges();

		var davidId = database.Users.FirstOrDefault(x => x.Name == "David").UserId;
		var avinashId = database.Users.FirstOrDefault(x => x.Name == "Avinash").UserId;

		var prog2Id = database.Subjects.FirstOrDefault(x => x.Name == prog2.Name).SubjectId;
		var dotnetId = database.Subjects.FirstOrDefault(x => x.Name == dotnet.Name).SubjectId;

		List<UserSubject> userSubjects =
		[
			new UserSubject
			{
				SubjectId = prog2Id,
				UserId = davidId,
				State = UserSubjectState.Coordinator
			},

			new UserSubject
			{
				SubjectId = dotnetId,
				UserId = davidId,
				State = UserSubjectState.Tutor
			},

			new UserSubject
			{
				SubjectId = dotnetId,
				UserId = avinashId,
				State = UserSubjectState.Coordinator
			}

		];

		database.UserSubjects.AddRange(userSubjects);
		database.SaveChanges();
	}
}