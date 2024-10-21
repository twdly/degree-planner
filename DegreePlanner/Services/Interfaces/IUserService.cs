using DegreePlanner.ViewModels;

namespace DegreePlanner.Services.Interfaces
{
    public interface IUserService
    {
        public UserViewModel GetUserFromId(int userId);
		MajorViewModel? GetUserMajor(int userId);
	}
}
