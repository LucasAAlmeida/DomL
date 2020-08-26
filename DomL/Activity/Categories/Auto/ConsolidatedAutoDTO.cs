﻿using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedAutoDTO : ConsolidatedActivityDTO
    {
        public string AutoName;
        public string Description;

        public ConsolidatedAutoDTO(Activity activity) : base(activity)
        {
            var autoActivity = activity.AutoActivity;
            var auto = autoActivity.Auto;

            AutoName = auto.Name;
            Description = autoActivity.Description;
        }

        public string GetInfoForYearRecap()
        {
            // Date Started; Date Finished;
            // Auto Name; Description
            return DatesStartAndFinish
                + "\t" + AutoName + "\t" + Description;
        }
    }
}