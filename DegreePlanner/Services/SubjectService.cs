using DegreePlanner.Data;
using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DegreePlanner.Services;

public class SubjectService(DatabaseContext databaseContext) : ISubjectService
{
	public void AddSubject(Subject subject)
	{
		databaseContext.Subjects.Add(subject);
		databaseContext.SaveChanges();
	}

	public List<SubjectViewModel> GetDegreeSubjectsToPlan(int userId)
	{
		var user = databaseContext.Users.Include(x => x.Degree).Include(x => x.Major).FirstOrDefault(x => x.UserId == userId);

		var currentEnrolmentIds = GetUserSubjectsWithState(UserSubjectState.Enrolled, userId).Select(x => x.SubjectId).ToList();
		var plannedSubjectIds = GetUserSubjectsWithState(UserSubjectState.Planned, userId).Select(x => x.SubjectId).ToList();
		var passedSubjectIds = GetUserSubjectsWithState(UserSubjectState.Passed, userId).Select(x => x.SubjectId).ToList();
		var degreeSubjects = databaseContext.DegreeSubjects.Where(x => x.DegreeId == user.Degree.DegreeId).ToList();
		var majorSubjects = databaseContext.MajorSubjects.Where(x => x.MajorId == user.Major.MajorId).ToList();

		degreeSubjects.RemoveAll(x => currentEnrolmentIds.Contains(x.SubjectId));
		List<SubjectViewModel> subjectViewModels = [];
		foreach (var degreeSubject in degreeSubjects)
		{
			var name = GetSubjectNameFromId(degreeSubject.SubjectId);
			var selected = plannedSubjectIds.Contains(degreeSubject.SubjectId);
			var hasPassed = passedSubjectIds.Contains(degreeSubject.SubjectId);

			subjectViewModels.Add(new SubjectViewModel(degreeSubject, name, selected, hasPassed));
		}

		majorSubjects.RemoveAll(x => currentEnrolmentIds.Contains(x.SubjectId));
		foreach (var majorSubject in majorSubjects)
		{
			var name = GetSubjectNameFromId(majorSubject.SubjectId);
			var selected = plannedSubjectIds.Contains(majorSubject.SubjectId);
			var hasPassed = passedSubjectIds.Contains(majorSubject.SubjectId);

			subjectViewModels.Add(new SubjectViewModel(majorSubject, name, selected, hasPassed));
		}

		return subjectViewModels;
	}

	private List<int> GetPrerequisitesForSubject(int subjectId)
	{
		var subject = databaseContext.Subjects.Include(x => x.Prerequisites).FirstOrDefault(x => x.SubjectId == subjectId);
		return subject.Prerequisites.Select(x => x.SubjectId).ToList();
	}

	private List<UserSubject> GetUserSubjectsWithState(UserSubjectState state, int userId)
	{
		return [.. databaseContext.UserSubjects.Where(x => x.UserId == userId && x.State == state)];
	}

	public string GetSubjectNameFromId(int subjectId)
	{
		return databaseContext.Subjects.Where(x => x.SubjectId == subjectId).Select(x => x.Name).SingleOrDefault();
	}

	public async void UpdateSubjects(List<SubjectViewModel> subjectViewModels, UserSubjectState state, int userId)
	{
		// Clear existing enrolments with selected state
		var currentUserSubjects = databaseContext.UserSubjects.Where(x => x.UserId == userId && x.State == state);
		databaseContext.UserSubjects.RemoveRange(currentUserSubjects);

		// If the user is enrolling in subjects, we need to also remove planned subjects that are being enrolled in
		// This prevents duplicate primary keys from being left in the UserSubjects table
		var updatedIds = subjectViewModels.Where(x => x.Selected != x.InitiallySelected).Select(x => x.SubjectId).ToList(); // Find all subjects that have been updated
		if (state == UserSubjectState.Enrolled)
		{
			var enrolledSubjects = databaseContext.UserSubjects.Where(x => x.UserId == userId && x.State == UserSubjectState.Planned && updatedIds.Contains(x.SubjectId));
			databaseContext.UserSubjects.RemoveRange(enrolledSubjects);
		}
		else if (state == UserSubjectState.Planned)
		{
			var failedSubjects = databaseContext.UserSubjects.Where(x => x.UserId == userId && x.State == UserSubjectState.Failed && updatedIds.Contains(x.SubjectId));
			databaseContext.UserSubjects.RemoveRange(failedSubjects); // Users need to be able to re-enrol in subjects that they have previously failed
		}
		await databaseContext.SaveChangesAsync();

		// Create EF classes from view models
		List<UserSubject> enrolments = [];
		foreach (var viewModel in subjectViewModels)
		{
			if (viewModel.Selected)
			{
				enrolments.Add(new UserSubject(viewModel, userId, state));
			}
			else if (state == UserSubjectState.Enrolled && updatedIds.Contains(viewModel.SubjectId))
			{
				enrolments.Add(new UserSubject(viewModel, userId, UserSubjectState.Planned)); // Ensure that subjects don't become unplanned after withdrawing
			}
		}
		databaseContext.UserSubjects.AddRange(enrolments);
		await databaseContext.SaveChangesAsync();
	}

	public List<SubjectViewModel> GetSubjectsToEnrol(int userId)
	{
		var userSubjects = databaseContext.UserSubjects
			.Where(x => x.UserId == userId && x.State == UserSubjectState.Enrolled || x.State == UserSubjectState.Planned)
			.ToList();

		var plannedSubjectIds = userSubjects.Where(x => x.State == UserSubjectState.Planned).Select(x => x.SubjectId).ToList();
		var enrolledSubjectIds = userSubjects.Where(x => x.State == UserSubjectState.Enrolled).Select(x => x.SubjectId).ToList();

		var subjects = databaseContext.Subjects.Where(x => plannedSubjectIds.Contains(x.SubjectId) || enrolledSubjectIds.Contains(x.SubjectId)).ToList();

		List<SubjectViewModel> subjectViewModels = [];
		foreach (var subject in subjects)
		{
			var prerequisites = GetPrerequisitesForSubject(subject.SubjectId);
			subjectViewModels.Add(new SubjectViewModel(subject, enrolledSubjectIds.Contains(subject.SubjectId), GetSubjectType(userId, subject.SubjectId), prerequisites));
		}
		return subjectViewModels;
	}

	public List<SubjectViewModel> GetPredictionSubjects(int userId)
	{
		// Subjects with a state of either planned or enrolled indicate that the student is yet to complete the subject
		var subjects = GetUserSubjectsWithState(UserSubjectState.Planned, userId);
		subjects.AddRange(GetUserSubjectsWithState(UserSubjectState.Enrolled, userId));

		List<SubjectViewModel> viewModels = [];
		foreach (var subject in subjects)
		{
			viewModels.Add(new SubjectViewModel(subject, GetSubjectNameFromId(subject.SubjectId)));
		}
		return viewModels;
	}

	public List<SubjectViewModel> GetCompletedSubjects(int userId)
	{
		var subjects = GetUserSubjectsWithState(UserSubjectState.Passed, userId);
		subjects.AddRange(GetUserSubjectsWithState(UserSubjectState.Failed, userId));

		List<SubjectViewModel> viewModels = [];
		foreach (var subject in subjects)
		{
			viewModels.Add(new SubjectViewModel(subject, GetSubjectNameFromId(subject.SubjectId)));
		}
		return viewModels;
	}

	private DegreeSubjectType GetSubjectType(int userId, int subjectId)
	{
		var user = databaseContext.Users.Include(x => x.Major).Include(x => x.Degree).FirstOrDefault(x => x.UserId == userId);
		var degreeSubject = databaseContext.DegreeSubjects.FirstOrDefault(x => x.DegreeId == user.Degree.DegreeId && x.SubjectId == subjectId);

		return degreeSubject?.Type ?? DegreeSubjectType.Major;
	}

	public List<SubjectViewModel> GetTeacherSubjects(int userId)
	{
		var teacherSubjects = databaseContext.UserSubjects.Where(x => x.UserId == userId && (x.State == UserSubjectState.Tutor || x.State == UserSubjectState.Coordinator)).ToList();

		List<SubjectViewModel> subjectViewModels = [];
		foreach (var subject in teacherSubjects)
		{
			subjectViewModels.Add(new SubjectViewModel(subject, GetSubjectNameFromId(subject.SubjectId)));
		}

		return subjectViewModels;
	}

	public List<SubjectViewModel> GetCoordinatedSubjects(int userId)
	{
		var coordinatedSubjects = databaseContext.UserSubjects.Where(x => x.UserId == userId && x.State == UserSubjectState.Coordinator).ToList();

		List<SubjectViewModel> subjectViewModels = [];
		foreach (var subject in coordinatedSubjects)
		{
			subjectViewModels.Add(new SubjectViewModel(subject, GetSubjectNameFromId(subject.SubjectId)));
		}

		return subjectViewModels;
	}

	public async void UpdateUserResultsForSubject(List<UserViewModel> updatedStudents, int subjectId)
	{
		// Remove enrolment details from the database
		var userIds = updatedStudents.Select(x => x.Id).ToList();
		var oldUserSubjects = databaseContext.UserSubjects.Where(x => userIds.Contains(x.UserId) && x.SubjectId == subjectId).ToList();
		databaseContext.UserSubjects.RemoveRange(oldUserSubjects);

		// Add results to the database
		List<UserSubject> userSubjects = [];
		foreach (var studentResult in updatedStudents)
		{
			userSubjects.Add(new UserSubject(studentResult.Id, subjectId, studentResult.Mark >= 50 ? UserSubjectState.Passed : UserSubjectState.Failed, studentResult.Mark));
		}

		databaseContext.AddRange(userSubjects);
		await databaseContext.SaveChangesAsync();
	}
}