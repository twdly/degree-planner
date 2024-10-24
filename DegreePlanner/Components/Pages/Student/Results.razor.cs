using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DegreePlanner.Components.Pages.Student;

public partial class Results
{
	[CascadingParameter]
	private Task<AuthenticationState> authenticationState { get; set; }
		
	[Inject]
	private ISubjectService SubjectService { get; set; }

	private List<SubjectViewModel> predictionSubjects;
	private List<SubjectViewModel> completedSubjects;
	private int userId;

	protected override async Task OnInitializedAsync()
	{
		var authState = await authenticationState;
		userId = int.Parse(authState.User.Identity.Name);
			
		predictionSubjects = SubjectService.GetPredictionSubjects(userId);
		completedSubjects = SubjectService.GetCompletedSubjects(userId);
	}

	private double GetCompletedWam()
	{
		var totalMarks = completedSubjects.Sum(x => x.Mark);
		if (completedSubjects.Count == 0) return 0;
		return Math.Round((double)totalMarks / completedSubjects.Count, 2);
	}

	private double GetPredictedWam()
	{
		predictionSubjects.ForEach(x => ValidateMarks(ref x.Mark));
		var predictedSubjects = predictionSubjects.Where(x => x.Mark != 0).ToList(); // Remove subjects with a mark of 0 as the user has not made a prediction
		var totalMarks = predictionSubjects.Sum(x => x.Mark);
		totalMarks += completedSubjects.Sum(x => x.Mark); // Completed subjects should still be counted in the prediction WAM
		var totalSubjects = predictedSubjects.Count + completedSubjects.Count;
		if (totalSubjects == 0) return 0;
		return Math.Round((double)totalMarks / totalSubjects, 2);
	}

	private static void ValidateMarks(ref int value)
	{
		switch (value)
		{
			case < 0:
				value = 0;
				break;
			case > 100:
				value = 100;
				break;
		}
	}

	private static string GetResultCss(bool isFail)
	{
		return isFail ? "color:red" : "color:green";
	}
}