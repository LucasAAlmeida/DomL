﻿using DomL.Business.Utils.Enums;
using System.Collections.Generic;
using System.IO;

namespace DomL.Business.Activities.SingleDayActivities
{
    public class Person : SingleDayActivity
    {
        readonly static Category categoria = Category.Person;

        public void Parse(IReadOnlyList<string> segmentos)
        {
            //PESSOA; (Assunto) Nome da Pessoa; (Descrição) Coisas pra me lembrar
            //PESSOA; (Assunto) Nome da Pessoa; (DeQuem) Origem conheci (amigo de x, furry, etc); (Descrição) Coisas pra me lembrar

            Categoria = categoria;
            Assunto = segmentos[1];
            if (segmentos.Count == 3)
            {
                Descricao = segmentos[2];
            }
            else
            {
                DeQuem = segmentos[2];
                Descricao = segmentos[3];
            }
        }

        protected override void ParseAtividadeVelha(string[] segmentos)
        {
            Assunto = segmentos[1];
            DeQuem = segmentos[2];
            Descricao = segmentos[3];
        }

        protected override string ConsolidateActivity()
        {
            string diaMes = Dia.Day.ToString("00") + "/" + Dia.Month.ToString("00");
            string deQuem = !string.IsNullOrWhiteSpace(DeQuem) ? DeQuem : "-";
            return diaMes + "\t" + Assunto + "\t" + deQuem + "\t" + Descricao;
        }
    }
}
