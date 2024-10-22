﻿using DegreePlanner.Data;
using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DegreePlanner.Services
{
	public class SubjectService(DatabaseContext databaseContext) : ISubjectService
	{
		public List<SubjectViewModel> GetDegreeSubjectsToPlan(int userId)
		{
			var user = databaseContext.Users.Include(x => x.Degree).Include(x => x.Major).FirstOrDefault(x => x.UserId == userId);

			var currentEnrolmentIds = GetUserSubjectsWithState(UserSubjectState.Enrolled, userId).Select(x => x.SubjectId).ToList();
			var plannedSubjectIds = GetUserSubjectsWithState(UserSubjectState.Planned, userId).Select(x => x.SubjectId).ToList();
			var degreeSubjects = databaseContext.DegreeSubjects.Where(x => x.DegreeId == user.Degree.DegreeId).ToList();
			var majorSubjects = databaseContext.MajorSubjects.Where(x => x.MajorId == user.Major.MajorId).ToList();

			degreeSubjects.RemoveAll(x => currentEnrolmentIds.Contains(x.SubjectId));
			List<SubjectViewModel> subjectViewModels = [];
			foreach (var degreeSubject in degreeSubjects)
			{
				subjectViewModels.Add(new(degreeSubject, GetSubjectNameFromId(degreeSubject.SubjectId), plannedSubjectIds.Contains(degreeSubject.SubjectId)));
			}

			majorSubjects.RemoveAll(x => currentEnrolmentIds.Contains(x.SubjectId));
			foreach (var majorSubject in majorSubjects)
			{
				subjectViewModels.Add(new(majorSubject, GetSubjectNameFromId(majorSubject.SubjectId), plannedSubjectIds.Contains(majorSubject.SubjectId)));
			}

			return subjectViewModels;
		}

		private List<UserSubject> GetUserSubjectsWithState(UserSubjectState state, int userId)
		{
			return [.. databaseContext.UserSubjects.Where(x => x.UserId == userId && x.State == state)];
		}

		private string GetSubjectNameFromId(int subjectId)
		{
			return databaseContext.Subjects.Where(x => x.SubjectId == subjectId).Select(x => x.Name).SingleOrDefault();
		}

		public async void UpdateSubjects(List<SubjectViewModel> subjectViewModels, UserSubjectState state, int userId)
		{
			// Clear existing enrolments with selected state
			var currentUserSubjects = databaseContext.UserSubjects.Where(x => x.UserId == userId && x.State == state);
			databaseContext.UserSubjects.RemoveRange(currentUserSubjects);

			// If the user is enrolling in subjects, we need to also remove planned subjects that are being enrolled in
			// This prevents duplicate primary keys from being left in the UserSubjects table
			if (state == UserSubjectState.Enrolled)
			{
				var viewModelIds = subjectViewModels.Select(x => x.SubjectId).ToList();
				var enrolledSubjects = databaseContext.UserSubjects.Where(x => x.UserId == userId && x.State == UserSubjectState.Planned && viewModelIds.Contains(x.SubjectId));
				databaseContext.UserSubjects.RemoveRange(enrolledSubjects);
			}
			await databaseContext.SaveChangesAsync();

			// Create EF classes from view models
			List<UserSubject> enrolments = [];
			foreach (var viewModel in subjectViewModels)
			{
				if (viewModel.Selected)
				{
					enrolments.Add(new(viewModel, userId, state));
				}
			}
			databaseContext.UserSubjects.AddRange(enrolments);
			await databaseContext.SaveChangesAsync();
		}

		public List<SubjectViewModel> GetSubjectsToEnrol(int userId)
		{
			var userSubjects = databaseContext.UserSubjects
				.Where(x => x.UserId == userId && x.State == UserSubjectState.Enrolled || x.State == UserSubjectState.Planned)
				.ToList();

			var plannedSubjectIds = userSubjects.Where(x => x.State == UserSubjectState.Planned).Select(x => x.SubjectId).ToList();
			var enrolledSubjectIds = userSubjects.Where(x => x.State == UserSubjectState.Enrolled).Select(x => x.SubjectId).ToList();

			var subjects = databaseContext.Subjects.Where(x => plannedSubjectIds.Contains(x.SubjectId) || enrolledSubjectIds.Contains(x.SubjectId)).ToList();

			List<SubjectViewModel> subjectViewModels = [];
			foreach (var subject in subjects)
			{
				subjectViewModels.Add(new(subject, enrolledSubjectIds.Contains(subject.SubjectId), GetSubjectType(userId, subject.SubjectId)));
			}
			return subjectViewModels;
		}

		private DegreeSubjectType GetSubjectType(int userId, int subjectId)
		{
			var user = databaseContext.Users.Include(x => x.Major).Include(x => x.Degree).FirstOrDefault(x => x.UserId == userId);
			var degreeSubject = databaseContext.DegreeSubjects.FirstOrDefault(x => x.DegreeId == user.Degree.DegreeId && x.SubjectId == subjectId);

			return degreeSubject != null ? degreeSubject.Type : DegreeSubjectType.Major;
		}
	}
}
