using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DegreePlanner.Components.Pages.Student;

public partial class MyDegree
{
	[Inject]
	private IDegreeService DegreeService { get; set; }

	[Inject]
	private IUserService UserService { get; set; }

	[CascadingParameter]
	private Task<AuthenticationState> authenticationState { get; set; }

	private DegreeViewModel? degreeViewModel;
	private List<DegreeViewModel>? availableDegrees;
	private List<MajorViewModel>? majors = [];
	private int selectedRadioId;
	private string enrolMessage = "";
	private MajorViewModel? userMajor;

	protected override async Task OnInitializedAsync()
	{
		var authState = await authenticationState;
		var userId = int.Parse(authState.User.Identity.Name); // User ID is stored in the name field of the authstate
		degreeViewModel = UserService.GetDegreeForUser(userId);
		if (degreeViewModel == null)
		{
			availableDegrees = DegreeService.GetAllDegrees();
		}
		else
		{
			userMajor = UserService.GetUserMajor(userId);
			if (userMajor == null)
			{
				majors = DegreeService.GetMajorsForDegree(degreeViewModel);
			}
		}
	}

	private void UpdateSelection(int selectionId)
	{
		selectedRadioId = selectionId;
	}

	private async void EnrolInDegree()
	{
		var authState = await authenticationState;
		DegreeService.EnrolInDegree(int.Parse(authState.User.Identity.Name), selectedRadioId);
		enrolMessage = "You have successfully enrolled!";
		await OnInitializedAsync();
	}

	private async void EnrolInMajor()
	{
		var authState = await authenticationState;
		DegreeService.EnrolInMajor(int.Parse(authState.User.Identity.Name), selectedRadioId);
		enrolMessage = "You have successfully selected a major!";
		majors = []; // Remove the list of majors to show that the user has now selected a major
		await OnInitializedAsync();
	}
}