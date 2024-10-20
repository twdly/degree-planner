using DegreePlanner.Data;
using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DegreePlanner.Services
{
	public class DegreeService(DatabaseContext databaseContext) : IDegreeService
	{
		public DegreeViewModel? GetDegreeForUser(int id)
		{
			var user = databaseContext.Users.Include(x => x.Degree).ThenInclude(x => x.Subjects).FirstOrDefault(x => x.UserId == id);
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
