using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DegreePlanner.Components.Pages.Teacher;

public partial class Tutors
{
	[CascadingParameter]
	private Task<AuthenticationState> AuthenticationState { get; set; }

	[Inject]
	private IUserService UserService { get; set; }

	[Inject]
	private ISubjectService SubjectService { get; set; }

	private List<SubjectViewModel> subjects;
	private List<TutorViewModel> tutors;
	private SubjectViewModel? selectedSubject;
	private int userId;
	private string message = "";

	protected override async Task OnInitializedAsync()
	{
		var authState = await AuthenticationState;
		userId = int.Parse(authState.User.Identity.Name);

		subjects = SubjectService.GetCoordinatedSubjects(userId);
	}

	private void SelectSubject(SubjectViewModel subject)
	{
		tutors = UserService.GetTutorsForSubject(userId, subject.SubjectId);
		selectedSubject = subject;
		message = "";
	}

	private void SaveTutors()
	{
		UserService.SaveTutorsForSubject(tutors, selectedSubject!.SubjectId);
		message = "Tutors have been saved successfully";
		Back();
	}

	private void Back()
	{
		tutors = [];
		selectedSubject = null;
	}
}