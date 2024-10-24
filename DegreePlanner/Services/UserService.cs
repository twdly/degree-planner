using DegreePlanner.Data;
using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DegreePlanner.Services;

public class UserService(DatabaseContext databaseContext) : IUserService
{
	public int AddUser(User user)
	{
		databaseContext.Users.Add(user);
		databaseContext.SaveChanges();
		return user.UserId;
	}

	public UserViewModel GetUserFromId(int userId)
	{
		var user = databaseContext.Users.FirstOrDefault(x => x.UserId == userId);
		return new UserViewModel(user);
	}

	public MajorViewModel? GetUserMajor(int userId)
	{
		var user = databaseContext.Users.Include(x => x.Major).ThenInclude(x => x.Subjects).FirstOrDefault(x => x.UserId == userId);
		return user.Major != null ? new MajorViewModel(user.Major) : null;
	}

	public DegreeViewModel? GetDegreeForUser(int id)
	{
		var user = databaseContext.Users.Include(x => x.Major).Include(x => x.Degree).ThenInclude(x => x.Subjects).FirstOrDefault(x => x.UserId == id);
		return user.Degree != null ? new DegreeViewModel(user.Degree) : null;
	}

	public SubjectEnrolmentViewModel GetSubjectEnrolment(int subjectId)
	{
		var enrolledStudentIds = databaseContext.UserSubjects
			.Where(x => x.State == UserSubjectState.Enrolled && x.SubjectId == subjectId)
			.Select(x => x.UserId)
			.ToList();

		var subjectName = databaseContext.Subjects.Include(x => x.Users).FirstOrDefault(x => x.SubjectId == subjectId)?.Name;

		var enrolledStudents = databaseContext.Users.Where(x => enrolledStudentIds.Contains(x.UserId));
		List<UserViewModel> enrolledStudentsViewModel = [];
		foreach (var student in enrolledStudents)
		{
			enrolledStudentsViewModel.Add(new UserViewModel(student));
		}

		var plannedCount = databaseContext.UserSubjects.Count(x => x.SubjectId == subjectId && x.State == UserSubjectState.Planned);

		return new SubjectEnrolmentViewModel(enrolledStudentsViewModel, plannedCount, subjectName!);
	}

	public List<TutorViewModel> GetTutorsForSubject(int coordinatorId, int subjectId)
	{
		var staff = databaseContext.Users.Where(x => x.UserId != coordinatorId && x.Role == UserRole.Staff).ToList();
		var currentTutorIds = databaseContext.UserSubjects.Where(x => x.SubjectId == subjectId && x.State == UserSubjectState.Tutor).Select(x => x.UserId).ToList();

		List<TutorViewModel> tutorViewModels = [];
		foreach (var tutor in staff)
		{
			tutorViewModels.Add(new TutorViewModel(tutor, currentTutorIds.Contains(tutor.UserId)));
		}

		return tutorViewModels;
	}

	public async void SaveTutorsForSubject(List<TutorViewModel> tutors, int subjectId)
	{
		var currentTutors = databaseContext.UserSubjects.Where(x => x.SubjectId == subjectId && x.State == UserSubjectState.Tutor).ToList();
		databaseContext.RemoveRange(currentTutors);

		var selectedTutors = tutors.Where(x => x.IsTutor).ToList();
		List<UserSubject> dbTutors = [];
		foreach (var tutor in selectedTutors)
		{
			dbTutors.Add(new UserSubject(tutor, subjectId));
		}

		databaseContext.AddRange(dbTutors);
		await databaseContext.SaveChangesAsync();
	}

	public List<int> GetCompletedSubjectIds(int userId)
	{
		return [.. databaseContext.UserSubjects.Where(x => x.UserId == userId && x.State == UserSubjectState.Passed).Select(x => x.SubjectId)];
	}
}