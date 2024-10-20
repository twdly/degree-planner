﻿using DegreePlanner.Data;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

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
			List<Subject> subjects = [prog1, prog2, dotnet, linearAlgebra, probability, brm];

			// Create majors containing these subjects
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

			// Create degrees containing majors and subjects
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

			// Create sample users
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
		}
	}
}
