﻿using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Activities.SingleDayActivities
{
    [Table("Doom")]
    public class Doom : SingleDayActivity
    {
        public Doom(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }
        public Doom() { }

        protected override void PopulateActivity(IReadOnlyList<string> segmentos)
        {
            // DOOM; (Descrição)

            this.Description = segmentos[1];
        }

        public override void Save()
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                if (unitOfWork.DoomRepo.Exists(b => b.Date == this.Date)) {
                    return;
                }

                unitOfWork.DoomRepo.Add(this);
                unitOfWork.Complete();
            }
        }

        public override string ParseToString()
        {
            return Util.GetDiaMes(this.Date) + "\t" + this.Description;
        }

        public static IEnumerable<Doom> GetAllFromMes(int mes, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.DoomRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
            }
        }

        public static void ConsolidateYear(string fileDir, int year)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allDoom = unitOfWork.DoomRepo.Find(b => b.Date.Year == year).ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Doom" + year + ".txt", allDoom.Cast<SingleDayActivity>().ToList());
            }
        }

        public static void ConsolidateAll(string fileDir)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allDoom = unitOfWork.DoomRepo.GetAll().ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Doom.txt", allDoom.Cast<SingleDayActivity>().ToList());
            }
        }

        public static void FullRestoreFromFile(string fileDir)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allDooms = GetDoomsFromFile(fileDir + "Doom.txt");
                unitOfWork.DoomRepo.AddRange(allDooms);
                unitOfWork.Complete();
            }
        }

        private static List<Doom> GetDoomsFromFile(string filePath)
        {
            if (!File.Exists(filePath)) {
                return null;
            }

            var dooms = new List<Doom>();
            using (var reader = new StreamReader(filePath)) {

                string line;
                while ((line = reader.ReadLine()) != null) {
                    var segmentos = Regex.Split(line, "\t");

                    // Data; (Descrição)

                    var doom = new Doom() {
                        Date = DateTime.Parse(segmentos[0]),
                        Description = segmentos[1],

                        DayOrder = 0,
                    };
                    dooms.Add(doom);
                }
            }
            return dooms;
        }
    }
}