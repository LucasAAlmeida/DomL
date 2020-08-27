﻿using DomL.Business.DTOs;
using DomL.Business.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Entities
{
    [Table("Activity")]
    public class Activity
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int DayOrder { get; set; }
        public int CategoryId { get; set; }
        public int? StatusId { get; set; }
        public int? PairedActivityId { get; set; }
        public int? ActivityBlockId { get; set; }
        [Required]
        public string OriginalLine { get; set; }
        
        [ForeignKey("CategoryId")]
        public ActivityCategory Category { get; set; }
        [ForeignKey("StatusId")]
        public ActivityStatus Status { get; set; }
        [ForeignKey("PairedActivityId")]
        public Activity PairedActivity { get; set; }
        [ForeignKey("ActivityBlockId")]
        public ActivityBlock ActivityBlock { get; set; }

        public virtual AutoActivity AutoActivity { get; set; }
        public virtual BookActivity BookActivity { get; set; }
        public virtual ComicActivity ComicActivity { get; set; }
        public virtual DoomActivity DoomActivity { get; set; }
        public virtual EventActivity EventActivity { get; set; }
        public virtual GameActivity GameActivity { get; set; }
        public virtual GiftActivity GiftActivity { get; set; }
        public virtual HealthActivity HealthActivity { get; set; }
        public virtual MovieActivity MovieActivity { get; set; }
        public virtual PetActivity PetActivity { get; set; }
        public virtual MeetActivity MeetActivity { get; set; }
        public virtual PlayActivity PlayActivity { get; set; }
        public virtual PurchaseActivity PurchaseActivity { get; set; }
        public virtual ShowActivity ShowActivity { get; set; }
        public virtual TravelActivity TravelActivity { get; set; }
        public virtual WorkActivity WorkActivity { get; set; }

        public void SaveFromRawLine(string rawLine, UnitOfWork unitOfWork)
        {
            var segments = Regex.Split(rawLine, "; ");
            switch (this.Category.Id) {
                case ActivityCategory.AUTO:     AutoService.SaveFromRawSegments(segments, this, unitOfWork);        break;
                case ActivityCategory.BOOK:     BookService.SaveFromRawSegments(segments, this, unitOfWork);        break;
                case ActivityCategory.COMIC:    ComicService.SaveFromRawSegments(segments, this, unitOfWork);       break;
                case ActivityCategory.DOOM:     DoomService.SaveFromRawSegments(segments, this, unitOfWork);        break;
                case ActivityCategory.EVENT:    EventService.SaveFromRawSegments(segments, this, unitOfWork);       break;
                case ActivityCategory.GAME:     GameService.SaveFromRawSegments(segments, this, unitOfWork);        break;
                case ActivityCategory.GIFT:     GiftService.SaveFromRawSegments(segments, this, unitOfWork);        break;
                case ActivityCategory.HEALTH:   HealthService.SaveFromRawSegments(segments, this, unitOfWork);      break;
                case ActivityCategory.MOVIE:    MovieService.SaveFromRawSegments(segments, this, unitOfWork);       break;
                case ActivityCategory.PET:      PetService.SaveFromRawSegments(segments, this, unitOfWork);         break;
                case ActivityCategory.MEET:     MeetService.SaveFromRawSegments(segments, this, unitOfWork);        break;
                case ActivityCategory.PLAY:     PlayService.SaveFromRawSegments(segments, this, unitOfWork);        break;
                case ActivityCategory.PURCHASE: PurchaseService.SaveFromRawSegments(segments, this, unitOfWork);    break;
                case ActivityCategory.SHOW:     ShowService.SaveFromRawSegments(segments, this, unitOfWork);        break;
                case ActivityCategory.TRAVEL:   TravelService.SaveFromRawSegments(segments, this, unitOfWork);      break;
                case ActivityCategory.WORK:     WorkService.SaveFromRawSegments(segments, this, unitOfWork);        break;
            }
        }

        public void PairUpActivity(UnitOfWork unitOfWork)
        {
            if (this.Status.Id != ActivityStatus.FINISH) {
                return;
            }

            var startingActivity = this.GetStartingActivity(unitOfWork);

            if (startingActivity != null) {
                this.PairedActivity = startingActivity;
                startingActivity.PairedActivity = this;
            }
        }

        private Activity GetStartingActivity(UnitOfWork unitOfWork)
        {
            var psa = unitOfWork.ActivityRepo.GetPreviousStartingActivities(this.Date);

            IEnumerable<Activity> pcsa = null; // Previous Category Starting Activities
            switch (this.Category.Id) {
                case ActivityCategory.AUTO:     pcsa = AutoService.GetStartingActivities(psa, this);      break;
                case ActivityCategory.BOOK:     pcsa = BookService.GetStartingActivities(psa, this);      break;
                case ActivityCategory.COMIC:    pcsa = ComicService.GetStartingActivities(psa, this);     break;
                case ActivityCategory.DOOM:     pcsa = DoomService.GetStartingActivities(psa, this);      break;
                case ActivityCategory.GAME:     pcsa = GameService.GetStartingActivities(psa, this);      break;
                case ActivityCategory.HEALTH:   pcsa = HealthService.GetStartingActivities(psa, this);    break;
                case ActivityCategory.MOVIE:    pcsa = MovieService.GetStartingActivities(psa, this);     break;
                case ActivityCategory.PET:      pcsa = PetService.GetStartingActivities(psa, this);       break;
                case ActivityCategory.SHOW:     pcsa = ShowService.GetStartingActivities(psa, this);     break;
                    //case ActivityCategory.WORK:     K": new Work(atividadeDTO, segmentos).Save(); break;
            }
            return pcsa.OrderByDescending(u => u.Date).FirstOrDefault();
        }

        public string GetInfoForMonthRecap()
        {
            var consolidated = new ConsolidatedActivityDTO(this);

            if (this.CategoryId == ActivityCategory.EVENT && this.ActivityBlock == null && !this.EventActivity.IsImportant) {
                return "";
            }

            return consolidated.GetInfoForMonthRecap();
        }

        public string GetInfoForYearRecap()
        {
            if (this.StatusId == ActivityStatus.START && this.PairedActivity != null) {
                return "";
            }

            switch (this.Category.Id) {
                case ActivityCategory.AUTO:     return new ConsolidatedAutoDTO(this).GetInfoForYearRecap();
                case ActivityCategory.BOOK:     return new ConsolidatedBookDTO(this).GetInfoForYearRecap();
                case ActivityCategory.COMIC:    return new ConsolidatedComicDTO(this).GetInfoForYearRecap();
                case ActivityCategory.DOOM:     return new ConsolidatedDoomDTO(this).GetInfoForYearRecap();
                case ActivityCategory.EVENT:    return new ConsolidatedEventDTO(this).GetInfoForYearRecap();
                case ActivityCategory.GAME:     return new ConsolidatedGameDTO(this).GetInfoForYearRecap();
                case ActivityCategory.GIFT:     return new ConsolidatedGiftDTO(this).GetInfoForYearRecap();
                case ActivityCategory.HEALTH:   return new ConsolidatedHealthDTO(this).GetInfoForYearRecap();
                case ActivityCategory.MOVIE:    return new ConsolidatedMovieDTO(this).GetInfoForYearRecap();
                case ActivityCategory.PET:      return new ConsolidatedPetDTO(this).GetInfoForYearRecap();
                case ActivityCategory.MEET:     return new ConsolidatedMeetDTO(this).GetInfoForYearRecap();
                case ActivityCategory.PLAY:     return new ConsolidatedPlayDTO(this).GetInfoForYearRecap();
                case ActivityCategory.PURCHASE: return new ConsolidatedPurchaseDTO(this).GetInfoForYearRecap();
                case ActivityCategory.SHOW:     return new ConsolidatedShowDTO(this).GetInfoForYearRecap();
                case ActivityCategory.TRAVEL:   return new ConsolidatedTravelDTO(this).GetInfoForYearRecap();
                    //case ActivityCategory.TRAVEL:   VEL": new Travel(atividadeDTO, segmentos).Save(); break;
                    //case ActivityCategory.WORK:     K": new Work(atividadeDTO, segmentos).Save(); break;
            }
            return "";
        }
    }

    [Table("ActivityBlock")]
    public class ActivityBlock
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
        //public int? Save()
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        if (unitOfWork.ActivityBlockRepo.Exists(b => b.Name == this.Name)) {
        //            return null;
        //        }

        //        unitOfWork.ActivityBlockRepo.Add(this);
        //        unitOfWork.Complete();

        //        this.Id = unitOfWork.ActivityBlockRepo.SingleOrDefault(abr => abr.Name == this.Name).Id;
        //        return this.Id;
        //    }
        //}

        //public static void ConsolidateYear(string fileDir, int year)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        var allActivitiesInBlocksInYear = unitOfWork.ActivityBlockRepo.GetActivitiesInBlockYear(year).ToList();
        //        using (var file = new StreamWriter(fileDir + "ActivityBlocks" + year + ".txt")) {
        //            foreach (var atividade in allActivitiesInBlocksInYear) {
        //                file.WriteLine(atividade.ParseToString());
        //            }
        //        }
        //    }
        //}

    [Table("ActivityCategory")]
    public class ActivityCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public const int AUTO = 1;
        public const int BOOK = 2;
        public const int COMIC = 3;
        public const int DOOM = 4;
        public const int GIFT = 5;
        public const int HEALTH = 6;
        public const int MOVIE = 7;
        public const int MEET = 8;
        public const int PET = 9;
        public const int PLAY = 10;
        public const int PURCHASE = 11;
        public const int TRAVEL = 12;
        public const int WORK = 13;
        public const int GAME = 14;
        public const int SHOW = 15;
        public const int EVENT = 17;
    }

    [Table("ActivityStatus")]
    public class ActivityStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public const int SINGLE = 1;
        public const int START = 2;
        public const int FINISH = 3;
    }
}
