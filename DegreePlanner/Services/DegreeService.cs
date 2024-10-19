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
	}
}
