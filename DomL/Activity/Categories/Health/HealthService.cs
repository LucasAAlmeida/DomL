﻿using DomL.Business.Entities;
using DomL.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Services
{
    public class HealthService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // HEALTH; (Medical Specialty Name); Description
            string specialtyName = null;
            string description;
            if (segments.Length == 2) {
                description = segments[1];
            } else {
                specialtyName = segments[1];
                description = segments[2];
            }

            Company specialty = CompanyService.GetOrCreateByName(specialtyName, unitOfWork);
            CreateHealthActivity(activity, specialty, description, unitOfWork);
        }

        private static void CreateHealthActivity(Activity activity, Company specialty, string description, UnitOfWork unitOfWork)
        {
            var healthActivity = new HealthActivity() {
                Activity = activity,
                Specialty = specialty,
                Description = description
            };

            activity.HealthActivity = healthActivity;
            ActivityService.PairUpWithStartingActivity(activity, unitOfWork);

            unitOfWork.HealthRepo.CreateHealthActivity(healthActivity);
        }

        public static IEnumerable<Activity> GetStartingActivities(IQueryable<Activity> previousStartingActivities, Activity activity)
        {
            var healthActivity = activity.HealthActivity;
            return previousStartingActivities.Where(u =>
                u.CategoryId == ActivityCategory.HEALTH_ID
                && u.HealthActivity.Description == healthActivity.Description
            );
        }

        public static void RestoreFromFile(string fileDir)
        {
            using (var reader = new StreamReader(fileDir + "Health.txt")) {
                string line = "";
                while ((line = reader.ReadLine()) != null) {
                    if (string.IsNullOrWhiteSpace(line)) {
                        continue;
                    }

                    var segments = Regex.Split(line, "\t");

                    // Date; (Medical Specialty Name); Description
                    var date = segments[0];
                    var specialtyName = segments[1] != "-" ? segments[1] : null;
                    var description = segments[2];

                    var originalLine = "HEALTH";
                    originalLine = (!string.IsNullOrWhiteSpace(specialtyName)) ? originalLine + "; " + specialtyName : originalLine;
                    originalLine += "; " + description;

                    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                        var specialty = CompanyService.GetOrCreateByName(specialtyName, unitOfWork);
                        var statusSingle = unitOfWork.ActivityRepo.GetStatusById(ActivityStatus.SINGLE);
                        var category = unitOfWork.ActivityRepo.GetCategoryById(ActivityCategory.HEALTH_ID);

                        var dateDT = DateTime.ParseExact(date, "dd/MM/yy", null);
                        var activity = ActivityService.Create(dateDT, 0, statusSingle, category, null, originalLine, unitOfWork);

                        CreateHealthActivity(activity, specialty, description, unitOfWork);

                        unitOfWork.Complete();
                    }
                }
            }
        }
    }
}
