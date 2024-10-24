using DegreePlanner.Data;
using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DegreePlanner.Components.Pages.Student;

public partial class Subjects
{
    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationState { get; set; }

    [Inject]
    public IUserService UserService { get; set; }

    [Inject]
    public ISubjectService SubjectService { get; set; }

    private List<SubjectViewModel> subjects = [];
    private List<int> completedSubjects = [];
    private bool typeSelected;
    private UserSubjectState enrolType;
    private DegreeViewModel? degree;
    private MajorViewModel? major;
    private int userId;
    private string message = "";

    private void SetEnrolType(UserSubjectState type)
    {
        enrolType = type;
    }

    private void LoadSubjects()
    {
        typeSelected = true;
        completedSubjects = UserService.GetCompletedSubjectIds(userId);
        subjects = enrolType == UserSubjectState.Planned ? SubjectService.GetDegreeSubjectsToPlan(userId) : SubjectService.GetSubjectsToEnrol(userId);
    }

    private void Back()
    {
        typeSelected = false;
    }

    private void UpdateSubjects()
    {
        SubjectService.UpdateSubjects(subjects, enrolType, userId);
        message = "Subjects have been updated!";
        Back();
    }

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationState;
        userId = int.Parse(authState.User.Identity.Name);
        degree = UserService.GetDegreeForUser(userId);
        major = UserService.GetUserMajor(userId);
    }

    private string GetSubjectTypeTitle()
    {
        return enrolType == UserSubjectState.Planned ? "Planning Subjects" : "Enrolling in Subjects";
    }

    private bool CanEnrol()
    {
        if (enrolType == UserSubjectState.Planned) return true;

        return subjects.Where(x => x.Selected).Count() <= 4;
    }

    private bool HasIncompletePrerequisites(List<int> prerequisiteIds)
    {
        return prerequisiteIds.Any(prerequisite => !completedSubjects.Contains(prerequisite));
    }
}