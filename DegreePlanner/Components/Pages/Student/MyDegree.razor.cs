using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DegreePlanner.Components.Pages.Student
{
	public partial class MyDegree
	{
		[Inject]
		IDegreeService DegreeService { get; set; }

		[Inject]
		IUserService UserService { get; set; }

		[CascadingParameter]
		private Task<AuthenticationState> authenticationState { get; set; }

		public DegreeViewModel? degree;
		public List<DegreeViewModel>? availableDegrees;
		public List<MajorViewModel>? majors = [];
		public int selectedRadioId = 0;
		public string enrolMessage = "";
		public MajorViewModel? userMajor;

		protected override async Task OnInitializedAsync()
		{
			var authState = await authenticationState;
			var userId = int.Parse(authState.User.Identity.Name); // User ID is stored in the name field of the authstate
			degree = DegreeService.GetDegreeForUser(userId);
			if (degree == null)
			{
				availableDegrees = DegreeService.GetAllDegrees();
			}
			else
			{
				userMajor = UserService.GetUserMajor(userId);
				if (userMajor == null)
				{
					majors = DegreeService.GetMajorsForDegree(degree);
				}
			}
		}

		private async void UpdateSelection(int selectionId)
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
}
