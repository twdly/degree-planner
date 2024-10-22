using DegreePlanner.Data;
using DegreePlanner.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace DegreePlanner.Components.Pages.Admin
{
	public partial class AddUser
	{
		[Inject]
		IUserService UserService { get; set; }

		[SupplyParameterFromForm]
		public UserDetails Details { get; set; }

		private string resultMessage = "";
		UserRole userRole = UserRole.Admin;
		int userID = 0;

		void SetUserRole(UserRole role)
		{
			userRole = role;
		}

		private void CreateUser()
		{
			if(Details.Name == "" || Details.Name == null)
			{
				resultMessage = "No name was inputted, please input a name";
				return;
			}
			else if(Details.Password == "" || Details.Password == null)
			{
				resultMessage = "No password was inputted, please input a password";
				return;
			}
			else if(userRole == UserRole.Admin)
			{
				resultMessage = "No user role was selected, please select a role";
				return;
			}

			User user = new()
			{
				Name = Details.Name,
				Password = Details.Password,
				Role = userRole,
			};

			userID = UserService.AddUser(user);
			resultMessage = "Successfully created " + user.Name + " as a " + user.Role + ". Their ID is " + userID;
		}

		public class UserDetails
		{
			public string? Name { get; set; }
			public string? Password { get; set; }
		}

		protected override Task OnInitializedAsync()
		{
			Details ??= new();
			return base.OnInitializedAsync();
		}
	}
}
