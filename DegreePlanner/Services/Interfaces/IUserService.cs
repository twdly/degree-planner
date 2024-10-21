using DegreePlanner.ViewModels;

namespace DegreePlanner.Services.Interfaces
{
    public interface IUserService
    {
		/**
		 * Gets the user object corresponding to the given ID
		 */
		public UserViewModel GetUserFromId(int userId);

		/**
		 * Returns the major of the given user or null if they have not yet selected a major
		 */
		MajorViewModel? GetUserMajor(int userId);

		/**
		* Returns the degree of the given user or null if they have not yet enrolled in a degree
		*/
		public DegreeViewModel? GetDegreeForUser(int id);
	}
}
