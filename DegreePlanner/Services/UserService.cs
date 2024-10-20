using DegreePlanner.Data;
using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;

namespace DegreePlanner.Services
{
	public class UserService(DatabaseContext databaseContext) : IUserService
	{
		public UserViewModel GetUserFromId(int userId)
		{
			var user = databaseContext.Users.FirstOrDefault(x => x.UserId == userId);
			return new UserViewModel(user);
		}
	}
}
