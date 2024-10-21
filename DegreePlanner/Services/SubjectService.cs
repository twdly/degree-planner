using DegreePlanner.Data;
using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DegreePlanner.Services
{
	public class SubjectService(DatabaseContext databaseContext) : ISubjectService
	{
		public List<SubjectViewModel> GetDegreeSubjectsToPlan(int userId)
		{
			var user = databaseContext.Users.Include(x => x.Degree).FirstOrDefault(x => x.UserId == userId);

			var currentEnrolmentIds = GetUserSubjectsWithState(UserSubjectState.Enrolled, userId).Select(x => x.SubjectId).ToList();
			var plannedSubjectIds = GetUserSubjectsWithState(UserSubjectState.Planned, userId).Select(x => x.SubjectId).ToList();
			var degreeSubjects = databaseContext.DegreeSubjects.Where(x => x.DegreeId == user.Degree.DegreeId).ToList();

			degreeSubjects.RemoveAll(x => currentEnrolmentIds.Contains(x.SubjectId));
			List<SubjectViewModel> subjectViewModels = [];
			foreach (var degreeSubject in degreeSubjects)
			{
				subjectViewModels.Add(new(degreeSubject, GetSubjectNameFromId(degreeSubject.SubjectId), plannedSubjectIds.Contains(degreeSubject.SubjectId)));
			}
			return subjectViewModels;
		}

		private List<UserSubject> GetUserSubjectsWithState(UserSubjectState state, int userId)
		{
			return [.. databaseContext.UserSubjects.Where(x => x.UserId == userId && x.State == state)];
		}

		private string GetSubjectNameFromId(int subjectId)
		{
			return databaseContext.Subjects.Where(x => x.SubjectId == subjectId).Select(x => x.Name).SingleOrDefault();
		}

		public async void UpdateSubjects(List<SubjectViewModel> subjectViewModels, UserSubjectState state, int userId)
		{
			// Clear existing enrolments with selected state
			var currentUserSubjects = databaseContext.UserSubjects.Where(x => x.UserId == userId && x.State == state);
			databaseContext.UserSubjects.RemoveRange(currentUserSubjects);

			// Create EF classes from view models
			List<UserSubject> enrolments = [];
			foreach (var viewModel in subjectViewModels)
			{
				if (viewModel.Selected)
				{
					enrolments.Add(new(viewModel, userId, state));
				}
			}
			databaseContext.UserSubjects.AddRange(enrolments);
			await databaseContext.SaveChangesAsync();
		}
	}
}
