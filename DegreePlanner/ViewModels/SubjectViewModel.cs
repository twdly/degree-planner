﻿using DegreePlanner.Data;

namespace DegreePlanner.ViewModels;

public class SubjectViewModel
{
	public int SubjectId { get; set; }
	public string Name { get; set; }
	public int SubjectCode { get; set; }
	public bool Selected; // This selected is used to bind to the checkbox state when enrolling/planning
	public bool InitiallySelected; // This selected is used to check if the state of Selected has changed when planning/enrolling
	public DegreeSubjectType Type;
	public UserSubjectState State;
	public List<int> PrerequisiteIds = [];
	public int Mark;

	public SubjectViewModel(Subject subject)
	{
		SubjectId = subject.SubjectId;
		Name = subject.Name!;
		SubjectCode = subject.SubjectCode;
	}

	public SubjectViewModel(UserSubject userSubject, string name)
	{
		SubjectId = userSubject.SubjectId;
		Name = name;
		Mark = userSubject.Mark;
		State = userSubject.State;
	}

	public SubjectViewModel(Subject subject, bool selected, DegreeSubjectType type, List<int> prerequisites)
	{
		SubjectId = subject.SubjectId;
		Name = subject.Name!;
		SubjectCode = subject.SubjectCode;
		Selected = selected;
		InitiallySelected = selected;
		Type = type;
		PrerequisiteIds = prerequisites;
	}

	public SubjectViewModel(DegreeSubject degreeSubject, string name, bool selected, bool hasPassed)
	{
		SubjectId = degreeSubject.SubjectId;
		Name = name;
		Selected = selected;
		InitiallySelected = selected;
		Type = degreeSubject.Type;
		State = hasPassed ? UserSubjectState.Passed : UserSubjectState.Failed;
	}

	public SubjectViewModel(MajorSubject majorSubject, string name, bool selected, bool hasPassed)
	{
		SubjectId = majorSubject.SubjectId;
		Name = name;
		Selected = selected;
		InitiallySelected = selected;
		Type = DegreeSubjectType.Major;
		State = hasPassed ? UserSubjectState.Passed : UserSubjectState.Failed;
	}
}