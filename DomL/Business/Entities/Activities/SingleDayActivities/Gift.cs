﻿using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Activities.SingleDayActivities
{
    [Table("Gift")]
    public class Gift : SingleDayActivity
    {
        [Required]
        public string DeQuem { get; set; }

        public Gift(ActivityDTO atividadeDTO, string[] segmentos) : base(atividadeDTO, segmentos) { }
        public Gift() { }

        protected override void PopulateActivity(IReadOnlyList<string> segmentos)
        {
            //GIFT; (Assunto) O que ganhei; (DeQuem) De quem ganhei o presente
            //GIFT; (Assunto) O que ganhei; (DeQuem) De quem ganhei o presente; (Descrição) o que aconteceu

            this.Subject = segmentos[1];
            this.DeQuem = segmentos[2];
            if (segmentos.Count == 4)
            {
                this.Description = segmentos[3];
            }
        }

        public override void Save()
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                if (unitOfWork.GiftRepo.Exists(b => b.Date == this.Date)) {
                    return;
                }

                unitOfWork.GiftRepo.Add(this);
                unitOfWork.Complete();
            }
        }

        public override string ParseToString()
        {
            return Util.GetDiaMes(this.Date) + "\t" + this.Subject + "\t" + this.DeQuem + "\t" + this.Description;
        }

        public static IEnumerable<Gift> GetAllFromMes(int mes, int ano)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                return unitOfWork.GiftRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
            }
        }

        public static void ConsolidateYear(string fileDir, int year)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allGift = unitOfWork.GiftRepo.Find(b => b.Date.Year == year).ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Gift" + year + ".txt", allGift.Cast<SingleDayActivity>().ToList());
            }
        }

        public static void ConsolidateAll(string fileDir)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allGift = unitOfWork.GiftRepo.GetAll().ToList();
                EscreveConsolidadasNoArquivo(fileDir + "Gift.txt", allGift.Cast<SingleDayActivity>().ToList());
            }
        }

        public static void FullRestoreFromFile(string fileDir)
        {
            using (var unitOfWork = new UnitOfWork(new DomLContext())) {
                var allGifts = GetGiftsFromFile(fileDir + "Gift.txt");
                unitOfWork.GiftRepo.AddRange(allGifts);
                unitOfWork.Complete();
            }
        }

        private static List<Gift> GetGiftsFromFile(string filePath)
        {
            if (!File.Exists(filePath)) {
                return null;
            }

            var gifts = new List<Gift>();
            using (var reader = new StreamReader(filePath)) {

                string line;
                while ((line = reader.ReadLine()) != null) {
                    var segmentos = Regex.Split(line, "\t");

                    // Data; (Assunto) O que ganhei; (DeQuem) De quem ganhei o presente; (Descrição) o que aconteceu

                    var gift = new Gift() {
                        Date = DateTime.Parse(segmentos[0]),
                        Subject = segmentos[1],
                        DeQuem = segmentos[2],
                        Description = segmentos[3],

                        DayOrder = 0,
                    };
                    gifts.Add(gift);
                }
            }
            return gifts;
        }
    }
}
