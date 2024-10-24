using DegreePlanner.Data;
using DegreePlanner.Services;
using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DegreePlanner.Components.Pages.Admin
{
	public partial class EditUserDetails
	{
		[Inject]
		IUserService UserService { get; set; }

		List<UserViewModel>? allUsers;
		UserViewModel? selectedUser = null;
		string? newName, newPassword, errorMessage;

		protected override async Task OnInitializedAsync()
		{
			//var authState = await AuthenticationState;
			allUsers = UserService.GetAllUsers();
		}

		public void SelectUser(UserViewModel chosenUser)
		{
			selectedUser = chosenUser;
			newName = chosenUser.Name;
			newPassword = chosenUser.Password;
		}

		public void SaveUser(UserViewModel user)
		{
			if(newName == null || newName == "")
			{
				errorMessage = "No name entered, please enter a name";
				return;
			}
			else if(newPassword == null || newPassword == "")
			{
				errorMessage = "No password entered, please enter a password";
				return;
			}
			selectedUser.Name = newName;
			selectedUser.Password = newPassword;
			UserService.UpdateUser(user);
			DeselectUser();
		}

		public void DeselectUser()
		{
			newName = null;
			newPassword = null;
			errorMessage = null;
			selectedUser = null;
		}
	}
}
