﻿using DomL.Business.Entities;

namespace DomL.Business.DTOs
{
    public class ConsolidatedMovieActivityDTO : ConsolidatedActivityDTO
    {
        public string Title;
        public string DirectorName;
        public string SeriesName;
        public string NumberInSeries;
        public string Score;
        public string Description;

        public ConsolidatedMovieActivityDTO(Activity activity) : base(activity)
        {
            var movieActivity = activity.MovieActivity;
            var movie = movieActivity.Movie;
            
            Title = movie.Title;
            DirectorName = (movie.Director != null) ? movie.Director.Name : "-";
            SeriesName = (movie.Series != null) ? movie.Series.Name : "-";
            NumberInSeries = (!string.IsNullOrWhiteSpace(movie.NumberInSeries)) ? movie.NumberInSeries : "-";
            Score = (!string.IsNullOrWhiteSpace(movie.Score)) ? movie.Score : "-";
            Description = (!string.IsNullOrWhiteSpace(movieActivity.Description)) ? movieActivity.Description : "-";
        }

        public string GetInfoForYearRecap()
        {
            // Date Started; Date Finished;
            // Title; Director Name; Series Name; Number In Series; Score; Description
            return DatesStartAndFinish
                + "\t" + Title + "\t" + DirectorName
                + "\t" + SeriesName + "\t" + NumberInSeries
                + "\t" + Score + "\t" + Description;
        }
    }
}
