﻿using DomL.Business.Entities;
using DomL.Business.Services;
using System;

namespace DomL.Business.DTOs
{
    public class ConsolidatedBookDTO : ConsolidatedActivityDTO
    {
        public string Title;
        public string AuthorName;
        public string SeriesName;
        public string NumberInSeries;
        public string ScoreValue;
        public string Description;

        public ConsolidatedBookDTO(Activity activity) : base(activity)
        {
            var bookActivity = activity.BookActivity;
            var book = bookActivity.Book;
            
            Title = book.Title;
            AuthorName = (book.Author != null) ? book.Author.Name : "-";
            SeriesName = (book.Series != null) ? book.Series.Name : "-";
            NumberInSeries = (!string.IsNullOrWhiteSpace(book.NumberInSeries)) ? book.NumberInSeries : "-";
            ScoreValue = (book.Score != null) ? book.Score.Value.ToString() : "-";
            Description = (!string.IsNullOrWhiteSpace(bookActivity.Description)) ? bookActivity.Description : "-";
        }

        public string GetInfoForYearRecap()
        {
            // Date Started; Date Finished;
            // Title; Author Name; Series Name; Number In Series; Score; Description
            return DatesStartAndFinish
                + "\t" + GetBookActivityInfo();
        }

        public new string GetInfoForBackup()
        {
            return base.GetInfoForBackup()
                + "\t" + GetBookActivityInfo();
        }

        public string GetBookActivityInfo()
        {
            return Title + "\t" + AuthorName
                + "\t" + SeriesName + "\t" + NumberInSeries
                + "\t" + ScoreValue + "\t" + Description;
        }
    }
}
