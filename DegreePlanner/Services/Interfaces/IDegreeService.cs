using DegreePlanner.ViewModels;

namespace DegreePlanner.Services.Interfaces
{
	public interface IDegreeService
	{
		/**
		 * Returns a complete list of all available degrees
		 */
		List<DegreeViewModel> GetAllDegrees();

		/**
		* Returns the degree of the given user or null if they have not yet enrolled in a degree
		*/
		public DegreeViewModel? GetDegreeForUser(int id);

		/**
		 * Gets all available majors for a given degree
		 */
		List<MajorViewModel> GetMajorsForDegree(DegreeViewModel degree);

		/**
		 * Enrols the user with the provided userId in the degree with the provided degreeId
		 */
		public void EnrolInDegree(int userId, int degreeId);

		/**
		 * Enrols the user with the provided userId in the major with the provided majorId
		*/
		void EnrolInMajor(int userId, int majorId);
	}
}
