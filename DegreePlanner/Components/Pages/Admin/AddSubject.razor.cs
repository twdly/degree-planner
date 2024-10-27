using DegreePlanner.Data;
using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.AspNetCore.Components;

namespace DegreePlanner.Components.Pages.Admin
{
	public partial class AddSubject
	{
		[Inject]
		ISubjectService SubjectService { get; set; }

		[Inject]
		IDegreeService DegreeService { get; set; }

		[Inject]
		IUserService UserService { get; set; }

		[SupplyParameterFromForm]
		public SubjectDetails Details { get; set; }

		public List<UserViewModel> allTeachers;
		private UserViewModel? coordinator;

		private string resultMessage = "";
		private bool createdSubjectDetails, attachingToDegree, attachingToMajor, error;
		Subject? createdSubject;
		DegreeSubjectType subjectType;

		private void CreateSubject()
		{
			if(Details.Name == null || Details.Name == "")
			{
				resultMessage = "No name was entered, please enter a name";
				error = true;
				return;
			}
			else if(Details.Code == null || !Details.Code.HasValue || Details.Code < 1)
			{
				resultMessage = "No valid code was entered, please enter a valid code";
				error = true;
				return;
			}
			else if (SubjectService.CheckAllSubjectCodes((int)Details.Code))
			{
				resultMessage = "Subject Code is already assigned to another subject. Please enter a new Subject Code";
				error = true;
				return;
			}
			createdSubject = new()
			{
				Name = Details.Name,
				SubjectCode = (int)Details.Code,
			};
		}

		private void SelectDegree()
		{
			CreateSubject();
			if (error)
			{
				error = false;
				return;
			}
			resultMessage = "";
			createdSubjectDetails = true;
			attachingToDegree = true;
		}

		private void SetSubjectType(DegreeSubjectType type)
		{
			subjectType = type;
		}

		private void SetCoordinator(UserViewModel Coordinator)
		{
			coordinator = Coordinator;
		}

		private void AttachToDegree(DegreeViewModel degree)
		{
			SubjectService.AddSubject(createdSubject, coordinator);
			DegreeService.AttachToDegree(degree, createdSubject.SubjectId, subjectType);
			ReturnToMenu();
			//resultMessage = "Subject " + subject.SubjectCode + " " + subject.Name + " was created";
		}

		private void SelectMajor()
		{
			CreateSubject();
			if (error)
			{
				error = false;
				return;
			}
			resultMessage = "";
			createdSubjectDetails = true;
			attachingToMajor = true;
		}

		private void AttachToMajor(MajorViewModel major)
		{
			SubjectService.AddSubject(createdSubject, coordinator);
			DegreeService.AttachToMajor(major, createdSubject.SubjectId);
			ReturnToMenu();
		}

		public class SubjectDetails
		{
			public string? Name { get; set; }
			public int? Code { get; set; }
		}

		private void ReturnToMenu()
		{
			resultMessage = $"Created {Details.Name}, {Details.Code}";
			Details = new();
			createdSubjectDetails = false;
			attachingToDegree = false;
			attachingToMajor = false;
			createdSubject = null;
			coordinator = null;
			subjectType = DegreeSubjectType.Major;
		}

		protected override Task OnInitializedAsync()
		{
			Details = new();
			createdSubjectDetails = false;
			attachingToDegree = false;
			attachingToMajor = false;
			createdSubject = null;
			subjectType = DegreeSubjectType.Major;
			allTeachers = UserService.GetAllStaff();
			coordinator = null;
			return base.OnInitializedAsync();
		}
	}
}
