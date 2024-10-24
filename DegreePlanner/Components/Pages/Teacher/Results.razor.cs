using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DegreePlanner.Components.Pages.Teacher;

public partial class Results
{
	[CascadingParameter]
	private Task<AuthenticationState> AuthenticationState { get; set; }

	[Inject]
	private IUserService UserService { get; set; }

	[Inject]
	private ISubjectService SubjectService { get; set; }

	private List<SubjectViewModel> subjects { get; set; }
	private List<UserViewModel> students { get; set; } = [];
	private int selectedSubjectId { get; set; }
	private string selectedSubjectName { get; set; } = "";
	private int userId;
	private string message;

	protected override async Task OnInitializedAsync()
	{
		var authState = await AuthenticationState;
		userId = int.Parse(authState.User.Identity.Name);

		subjects = SubjectService.GetTeacherSubjects(userId);
		selectedSubjectId = subjects[0].SubjectId; // Set the selected subject ID to the value of the initially selected subject
	}

	private void SelectSubject()
	{
		students = UserService.GetSubjectEnrolment(selectedSubjectId).Students;
		selectedSubjectName = SubjectService.GetSubjectNameFromId(selectedSubjectId);
		message = ""; // Remove the successful enrolment message for if results have already been allocated
	}

	private void DeselectSubject()
	{
		students = [];
		selectedSubjectName = "";
		selectedSubjectId = subjects[0].SubjectId;
	}

	private void SaveResults()
	{
		var updatedStudents = students.Where(x => x.Mark is > 0 and <= 100).ToList();
		SubjectService.UpdateUserResultsForSubject(updatedStudents, selectedSubjectId);
		message = $"Results have been saved for {selectedSubjectName}";
		DeselectSubject();
	}
}