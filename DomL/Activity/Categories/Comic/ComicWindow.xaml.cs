﻿using DomL.Business.Entities;
using DomL.Business.Services;
using DomL.Business.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DomL.Presentation
{
    /// <summary>
    /// Interaction logic for ComicWindow.xaml
    /// </summary>
    public partial class ComicWindow : Window
    {
        private readonly UnitOfWork UnitOfWork;

        enum NamedIndices
        {
            title = 0,
            type = 1,
            series = 2,
            number = 3,
            person = 4,
            company = 5,
            year = 6,
            score = 7,
            description = 8
        }

        public ComicWindow(string[] segments, Activity activity, UnitOfWork unitOfWork)
        {
            InitializeComponent();

            UnitOfWork = unitOfWork;

            InfoMessage.Content = activity.GetInfoMessage();
            Util.FillSegmentosStack(segments, SegmentosStack);

            var comics = ComicService.GetAll(unitOfWork);

            var titleList = comics.Select(u => u.Title).Distinct().ToList();
            var typeList = comics.Where(u => u.Type != null).Select(u => u.Type).Distinct().ToList();
            var seriesList = comics.Where(u => u.Series != null).Select(u => u.Series.Name).Distinct().ToList();
            var numberList = Util.GetDefaultChaptersList();
            var personList = comics.Where(u => u.Person != null).Select(u => u.Person).Distinct().ToList();
            var companyList = comics.Where(u => u.Company != null).Select(u => u.Company).Distinct().ToList();
            var yearList = Util.GetDefaultYearList();
            var scoreList = Util.GetDefaultScoreList();

            segments[0] = "";
            var remainingSegments = segments;
            var orderedSegments = new string[Enum.GetValues(typeof(NamedIndices)).Length];

            var indexesToAvoid = new int[] { (int)NamedIndices.year };
            Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.title, remainingSegments[1], indexesToAvoid);
            Util.SetComboBox(TitleCB, segments, titleList, orderedSegments[(int)NamedIndices.title]);
            TitleCB_LostFocus(null, null);

            // COURSE; Title; Type; Series; Number; Person; Company; Year; Score; Description
            while (remainingSegments.Length > 2 && orderedSegments.Any(u => u == null)) {
                var searched = remainingSegments[2];
                if (int.TryParse(searched, out int number)) {
                    searched = number.ToString("00");
                }

                if (Util.ListContainsText(typeList, searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.type, searched, indexesToAvoid);
                } else if (Util.ListContainsText(seriesList, searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.series, searched, indexesToAvoid);
                } else if (Util.ListContainsText(numberList, searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.number, searched, indexesToAvoid);
                } else if (Util.ListContainsText(personList, searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.person, searched, indexesToAvoid);
                } else if (Util.ListContainsText(companyList, searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.company, searched, indexesToAvoid);
                } else if (Util.ListContainsText(yearList, searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.year, searched, indexesToAvoid);
                } else if (Util.ListContainsText(scoreList, searched)) {
                    Util.PlaceOrderedSegment(orderedSegments, (int)NamedIndices.score, searched, indexesToAvoid);
                } else {
                    Util.PlaceStringInFirstAvailablePosition(orderedSegments, indexesToAvoid, searched);
                }

                remainingSegments = remainingSegments.Where(u => u != remainingSegments[2]).ToArray();
            }

            Util.SetComboBox(TypeCB, segments, typeList, orderedSegments[(int)NamedIndices.type]);
            Util.SetComboBox(SeriesCB, segments, seriesList, orderedSegments[(int)NamedIndices.series]);
            Util.SetComboBox(NumberCB, segments, numberList, orderedSegments[(int)NamedIndices.number]);
            Util.SetComboBox(PersonCB, segments, personList, orderedSegments[(int)NamedIndices.person]);
            Util.SetComboBox(CompanyCB, segments, companyList, orderedSegments[(int)NamedIndices.company]);
            Util.SetComboBox(YearCB, segments, yearList, orderedSegments[(int)NamedIndices.year]);
            Util.SetComboBox(ScoreCB, segments, scoreList, orderedSegments[(int)NamedIndices.score]);
            Util.SetComboBox(DescriptionCB, segments, new List<string>(), orderedSegments[(int)NamedIndices.description]);
        }

        private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void TitleCB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TitleCB.IsKeyboardFocusWithin) {
                return;
            }

            var title = TitleCB.Text;
            var course = CourseService.GetByTitle(title, UnitOfWork);
            Util.ChangeInfoLabel(title, course, TitleInfoLb);

            if (course != null) {
                UpdateOptionalComboBoxes(course);
            }
        }

        private void UpdateOptionalComboBoxes(Course course)
        {
            if (course.Type != null) {
                TypeCB.Text = course.Type;
            }

            if (course.Series != null) {
                SeriesCB.Text = course.Series;
            }

            if (course.Number != null) {
                NumberCB.Text = course.Number;
            }

            if (course.Person != null) {
                PersonCB.Text = course.Person;
            }

            if (course.Company != null) {
                CompanyCB.Text = course.Company;
            }

            if (course.Year != 0) {
                YearCB.Text = course.Year.ToString();
            }

            if (course.Score != null) {
                ScoreCB.Text = course.Score;
            }
        }
    }
}
