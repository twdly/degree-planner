using DegreePlanner.Services;
using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.AspNetCore.Components;

namespace DegreePlanner.Components.Pages.Admin
{
	public partial class EditSubjectDetails
	{
		[Inject]
		ISubjectService SubjectService { get; set; }

		SubjectViewModel? selectedSubject = null;
		List<SubjectViewModel>? allSubjects;
		string? newName, errorMessage;
		int? newCode;

		protected override async Task OnInitializedAsync()
		{
			//var authState = await AuthenticationState;
			allSubjects = SubjectService.GetAllSubjects();
		}

		public void SelectSubject(SubjectViewModel chosenSubject)
		{
			selectedSubject = chosenSubject;
			newName = chosenSubject.Name;
			newCode = chosenSubject.SubjectCode;
		}

		public void SaveSubject(SubjectViewModel subject)
		{
			if (newName == null || newName == "")
			{
				errorMessage = "No name entered, please enter a name";
				return;
			}
			else if (newCode == null || newCode < 1)
			{
				errorMessage = "No valid Subject Code entered, please enter a valid Subject Code";
				return;
			}
			else if (SubjectService.CheckAllSubjectCodes((int)newCode))
			{
				if(newCode != selectedSubject.SubjectCode)
				{
					errorMessage = "Subject Code is already assigned to another subject. Please enter another Subject Code";
					return;
				}
			}
			selectedSubject.Name = newName;
			selectedSubject.SubjectCode = (int)newCode;
			SubjectService.UpdateSubject(subject);
			DeselectSubject();
		}

		public void DeselectSubject()
		{
			newName = null;
			newCode = null;
			errorMessage = null;
			selectedSubject = null;
		}
	}
}
