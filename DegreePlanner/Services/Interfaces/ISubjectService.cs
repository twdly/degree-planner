using DegreePlanner.Data;
using DegreePlanner.ViewModels;

namespace DegreePlanner.Services.Interfaces
{
	public interface ISubjectService
	{
		/**
		 * Returns a list of all available subjects the user is yet to enrol in with the "planned" bool set accordingly
		 */
		List<SubjectViewModel> GetDegreeSubjectsToPlan(int userId);

		/**
		 * Update the user state for each of the provided subjects
		 */
		void UpdateSubjects(List<SubjectViewModel> subjectViewModels, UserSubjectState state, int userId);
	}
}
