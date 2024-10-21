using DegreePlanner.Data;
using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DegreePlanner.Services
{
	public class UserService(DatabaseContext databaseContext) : IUserService
	{
		public UserViewModel GetUserFromId(int userId)
		{
			var user = databaseContext.Users.FirstOrDefault(x => x.UserId == userId);
			return new UserViewModel(user);
		}

		public MajorViewModel? GetUserMajor(int userId)
		{
			var user = databaseContext.Users.Include(x => x.Major).ThenInclude(x => x.Subjects).FirstOrDefault(x => x.UserId == userId);
			return user.Major != null ? new(user.Major) : null;
		}
	}
}
