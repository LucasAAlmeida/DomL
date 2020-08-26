﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomL.Business.Entities
{
    [Table("AutoActivity")]
    public class AutoActivity
    {
        [Key]
        [ForeignKey("Activity")]
        public int Id { get; set; }
        [ForeignKey("Auto")]
        public int AutoId { get; set; }
        [Required]
        public string Description { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Auto Auto { get; set; }
    }

    [Table("Auto")]
    public class Auto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }

        //protected override void PopulateActivity(IReadOnlyList<string> segmentos)
        //{
        //    

        //    this.Subject = segmentos[1];
        //    this.Description = segmentos[2];
        //}

        //public override void Save()
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        if (unitOfWork.AutoRepo.Exists(b => b.Date == this.Date)) {
        //            return;
        //        }

        //        unitOfWork.AutoRepo.Add(this);
        //        unitOfWork.Complete();
        //    }
        //}

        //public override string ParseToString()
        //{
        //    return Util.GetDiaMes(this.Date) + "\t" + this.Subject + "\t" + this.Description;
        //}

        //public static IEnumerable<Auto> GetAllFromMes(int mes, int ano)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        return unitOfWork.AutoRepo.Find(b => b.Date.Month == mes && b.Date.Year == ano);
        //    }
        //}

        //public static void ConsolidateYear(string fileDir, int year)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        var allAutos = unitOfWork.AutoRepo.Find(b => b.Date.Year == year).ToList();
        //        EscreveConsolidadasNoArquivo(fileDir + "Auto" + year + ".txt", allAutos.Cast<SingleDayActivity>().ToList());
        //    }
        //}

        //public static void ConsolidateAll(string fileDir)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        var allAutos = unitOfWork.AutoRepo.GetAll().ToList();
        //        EscreveConsolidadasNoArquivo(fileDir + "Auto.txt", allAutos.Cast<SingleDayActivity>().ToList());
        //    }
        //}

        //public static void FullRestoreFromFile(string fileDir)
        //{
        //    using (var unitOfWork = new UnitOfWork(new DomLContext())) {
        //        var allAutos = GetAutosFromFile(fileDir + "Auto.txt");
        //        unitOfWork.AutoRepo.AddRange(allAutos);
        //        unitOfWork.Complete();
        //    }
        //}

        //private static List<Auto> GetAutosFromFile(string filePath)
        //{
        //    if (!File.Exists(filePath)) {
        //        return null;
        //    }

        //    var autos = new List<Auto>();
        //    using (var reader = new StreamReader(filePath)) {

        //        string line;
        //        while ((line = reader.ReadLine()) != null) {
        //            var segmentos = Regex.Split(line, "\t");

        //            // Data; (Assunto) Qual automovel; (Descricao) O que Aconteceu

        //            var auto = new Auto() {
        //                Date = DateTime.Parse(segmentos[0]),
        //                Subject = segmentos[1],
        //                Description = segmentos[2],

        //                DayOrder = 0,
        //            };
        //            autos.Add(auto);
        //        }
        //    }
        //    return autos;
        //}
}