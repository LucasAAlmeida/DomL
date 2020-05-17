﻿using DomL.Business.Utils;
using DomL.Business.Utils.DTOs;
using DomL.Business.Utils.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Activities.MultipleDayActivities
{
    public class Comic : MultipleDayActivity
    {
        readonly static Category categoria = Category.Comic;

        public void Parse(IReadOnlyList<string> segmentos)
        {
            // COMIC|MANGA; (Assunto) Título; (Valor) Nota
            // COMIC|MANGA; (Assunto) Título; (Classificação) Começo
            // COMIC|MANGA; (Assunto) Título; (Valor) Nota; (Descrição) O que achei
            // COMIC|MANGA; (Assunto) Título; (Classificação) Término; (Valor) Nota
            // COMIC|MANGA; (Assunto) Título; (Classificação) Término; (Valor) Nota; (Descrição) O que achei

            Categoria = categoria;
            Assunto = segmentos[1];
            string segmentoToLower = segmentos[2].ToLower();
            string classificacao = "unica";
            switch (segmentos.Count)
            {
                case 3:
                    if (segmentoToLower == "comeco" || segmentoToLower == "começo")
                    {
                        classificacao = segmentoToLower;
                    }
                    else
                    {
                        Valor = segmentos[2];
                    }
                    break;
                case 4:
                    if (segmentoToLower == "termino" || segmentoToLower == "término")
                    {
                        classificacao = segmentoToLower;
                        Valor = segmentos[3];
                    }
                    else
                    {
                        Valor = segmentos[2];
                        Descricao = segmentos[3];
                    }
                    break;
                case 5:
                    classificacao = segmentos[2].ToLower();
                    Valor = segmentos[3];
                    Descricao = segmentos[4];
                    break;
                default:
                    throw new Exception("what");
            }

            switch (classificacao)
            {
                case "comeco": case "começo": Classificacao = Classification.Comeco; break;
                case "termino": case "término": Classificacao = Classification.Termino; break;
                case "unica": Classificacao = Classification.Unica; break;
                default: throw new Exception("what");
            }

            if (Classificacao != Classification.Comeco && Valor != "-" && !int.TryParse(Valor, out _))
            {
                throw new Exception("what");
            }
        }

        protected override void ParseAtividadeVelha(string[] segmentos)
        {
            Assunto = segmentos[2];
            Valor = segmentos[3];
            Descricao = segmentos[4];
        }

        protected override void WriteAtividadeConsolidada(StreamWriter file, string dataInicio, string dataTermino)
        {
            file.WriteLine(dataInicio + "\t" + dataTermino + "\t" + Assunto + "\t" + Valor + "\t" + Descricao);
        }
    }
}