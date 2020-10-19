﻿using DomL.Business.Entities;
using DomL.Presentation;

namespace DomL.Business.DTOs
{
    public class ConsolidatedCourseDTO : ConsolidatedActivityDTO
    {
        public string Name;
        public string SchoolName;
        public string TeacherName;
        public string ScoreValue;
        public string Description;

        public ConsolidatedCourseDTO(Activity activity) : base (activity)
        {
            CategoryName = "COURSE";

            var courseActivity = activity.CourseActivity;
            var course = courseActivity.Course;

            Name = course.Name;
            SchoolName = (course.School != null) ? course.School.Name : "-";
            TeacherName = (course.Teacher != null) ? course.Teacher.Name : "-";
            ScoreValue = (course.Score != null) ? course.Score.Value.ToString() : "-";
            Description = (!string.IsNullOrWhiteSpace(courseActivity.Description)) ? courseActivity.Description : "-";
        }

        public ConsolidatedCourseDTO(CourseWindow courseWindow, Activity activity) : base(activity)
        {
            CategoryName = "COURSE";

            Name = courseWindow.NameCB.Text;
            TeacherName = courseWindow.TeacherCB.Text;
            SchoolName = courseWindow.SchoolCB.Text;
            ScoreValue = courseWindow.ScoreCB.Text;
            Description = (!string.IsNullOrWhiteSpace(courseWindow.DescriptionCB.Text)) ? courseWindow.DescriptionCB.Text : null;
        }

        public ConsolidatedCourseDTO(string[] backupSegments) : base(backupSegments)
        {
            CategoryName = "COURSE";

            Name = backupSegments[4];
            SchoolName = backupSegments[5];
            TeacherName = backupSegments[6];
            ScoreValue = backupSegments[7];
            Description = backupSegments[8];

            OriginalLine = GetInfoForOriginalLine() + "; "
                + GetCourseActivityInfo().Replace("\t", "; ");
        }

        public new string GetInfoForYearRecap()
        {
            return base.GetInfoForYearRecap()
                + "\t" + GetCourseActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetCourseActivityInfo();
        }

        public string GetCourseActivityInfo()
        {
            return Name + "\t" + SchoolName + "\t" + TeacherName
                + "\t" + ScoreValue + "\t" + Description;
        }
    }
}
