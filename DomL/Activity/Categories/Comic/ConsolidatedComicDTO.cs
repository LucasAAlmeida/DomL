﻿using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedComicDTO : ConsolidatedActivityDTO
    {
        public string SeriesName;
        public string Chapters;
        public string AuthorName;
        public string Type;
        public string Score;
        public string Description;

        public ConsolidatedComicDTO(Activity activity) : base (activity)
        {
            var comicActivity = activity.ComicActivity;
            var comicVolume = comicActivity.ComicVolume;

            SeriesName = comicVolume.Series.Name;
            Chapters = comicVolume.Chapters;
            AuthorName = (comicVolume.Author != null) ? comicVolume.Author.Name : "-";
            Type = (comicVolume.Type != null) ? comicVolume.Type.Name : "-";
            Score = (comicVolume.Score != null) ? comicVolume.Score.Value.ToString() : "-";
            Description = (!string.IsNullOrWhiteSpace(comicActivity.Description)) ? comicActivity.Description : "-";
        }

        public string GetInfoForYearRecap()
        {
            // Date Started; Date Finished;
            // Series Name; Chapters; Author Name; Media Type Name; Score; Description
            return DatesStartAndFinish
                + "\t" + GetComicActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetComicActivityInfo();
        }

        public string GetComicActivityInfo()
        {
            return SeriesName + "\t" + Chapters
                + "\t" + AuthorName + "\t" + Type
                + "\t" + Score + "\t" + Description;
        }
    }
}
