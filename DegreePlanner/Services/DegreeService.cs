using DegreePlanner.Data;
using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DegreePlanner.Services
{
	public class DegreeService(DatabaseContext databaseContext) : IDegreeService
	{
		public void EnrolInDegree(int userId, int degreeId)
		{
			var user = databaseContext.Users.FirstOrDefault(x => x.UserId == userId);
			var degree = databaseContext.Degrees.FirstOrDefault(x => x.DegreeId == degreeId);
			user.Degree = degree;
			databaseContext.SaveChanges();
		}

		public void EnrolInMajor(int userId, int majorId)
		{
			var user = databaseContext.Users.FirstOrDefault(x => x.UserId == userId);
			var major = databaseContext.Majors.FirstOrDefault(x => x.MajorId == majorId);
			user.Major = major;
			databaseContext.SaveChanges();
		}

		public List<DegreeViewModel> GetAllDegrees()
		{
			var degrees = databaseContext.Degrees.Include(x => x.Subjects).Include(x => x.Majors).ToList();
			List<DegreeViewModel> viewModels = [];
			foreach (var degree in degrees)
			{
				viewModels.Add(new(degree));
			}
			return viewModels;
		}

		public DegreeViewModel? GetDegreeForUser(int id)
		{
			var user = databaseContext.Users.Include(x => x.Major).Include(x => x.Degree).ThenInclude(x => x.Subjects).FirstOrDefault(x => x.UserId == id);
			return user.Degree != null ? new(user.Degree) : null;
		}

		public List<MajorViewModel> GetMajorsForDegree(DegreeViewModel degree)
		{
			var queriedDegree = databaseContext.Degrees.Include(x => x.Majors).ThenInclude(x => x.Subjects).FirstOrDefault(x => x.DegreeId == degree.Id);
			var majors = queriedDegree.Majors;
			List<MajorViewModel> viewModels = [];
			foreach (var major in majors)
			{
				viewModels.Add(new(major));
			}
			return viewModels;
		}
	}
}
