﻿using DomL.Business.DTOs;
using DomL.Business.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DomL.Business.Services
{
    public class PlayService
    {
        public static void SaveFromRawSegments(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            // PLAY; Person Name; Description
            var personName = segments[1];
            var description = segments[2];

            Person person = PersonService.GetOrCreateByName(personName, unitOfWork);

            CreatePlayActivity(activity, person, description, unitOfWork);
        }

        private static void CreatePlayActivity(Activity activity, Person person, string description, UnitOfWork unitOfWork)
        {
            var playActivity = new PlayActivity() {
                Activity = activity,
                Person = person,
                Description = description
            };

            activity.PlayActivity = playActivity;

            unitOfWork.PlayRepo.CreatePlayActivity(playActivity);
        }
    }
}