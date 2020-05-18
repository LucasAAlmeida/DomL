﻿using DomL.Business.Activities.SingleDayActivities;
using DomL.Business.Utils;
using DomL.Business.Utils.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DomL.Business.Activities
{
    public abstract class SingleDayActivity : Activity
    {
        public static List<Activity> Consolidate(Category category, List<Activity> newCategoryActivities, string fileDir, int ano)
        {
            var filePath = fileDir + category.ToString() + ".txt";
            var atividadesVelhas = GetAtividadesVelhas(filePath, ano, category);
            atividadesVelhas.AddRange(Util.GetAtividadesToAdd(newCategoryActivities, atividadesVelhas));
            
            var allCategoryActivities = atividadesVelhas; 
            EscreverNoArquivo(filePath, allCategoryActivities, category);

            return allCategoryActivities;
        }

        private static List<Activity> GetAtividadesVelhas(string filePath, int year, Category category)
        {
            var atividadesVelhas = new List<Activity>();

            if (File.Exists(filePath))
            {
                using (var reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Replace("\t", ";");
                        var segmentos = Regex.Split(line, ";");

                        Activity atividadeVelha = Util.GetAtividadeVelha(segmentos[0], year, category, Classification.Unica);

                        switch (category)
                        {
                            case Category.Auto:
                                Auto auto = (Auto)atividadeVelha;
                                auto.ParseAtividadeVelha(segmentos);
                                atividadesVelhas.Add(auto);
                                break;
                            case Category.Doom:
                                Doom doom = (Doom)atividadeVelha;
                                doom.ParseAtividadeVelha(segmentos);
                                atividadesVelhas.Add(doom);
                                break;
                            case Category.Gift:
                                Gift gift = (Gift)atividadeVelha;
                                gift.ParseAtividadeVelha(segmentos);
                                atividadesVelhas.Add(gift);
                                break;
                            case Category.Health:
                                Health health = (Health)atividadeVelha;
                                health.ParseAtividadeVelha(segmentos);
                                atividadesVelhas.Add(health);
                                break;
                            case Category.Person:
                                Person person = (Person)atividadeVelha;
                                person.ParseAtividadeVelha(segmentos);
                                atividadesVelhas.Add(person);
                                break;
                            case Category.Pet:
                                Pet pet = (Pet)atividadeVelha;
                                pet.ParseAtividadeVelha(segmentos);
                                atividadesVelhas.Add(pet);
                                break;
                            case Category.Play:
                                Play play = (Play)atividadeVelha;
                                play.ParseAtividadeVelha(segmentos);
                                atividadesVelhas.Add(play);
                                break;
                            case Category.Purchase:
                                Purchase purchase = (Purchase)atividadeVelha;
                                purchase.ParseAtividadeVelha(segmentos);
                                atividadesVelhas.Add(purchase);
                                break;
                            case Category.Travel:
                                Travel travel = (Travel)atividadeVelha;
                                travel.ParseAtividadeVelha(segmentos);
                                atividadesVelhas.Add(travel);
                                break;
                            case Category.Work:
                                Work work = (Work)atividadeVelha;
                                work.ParseAtividadeVelha(segmentos);
                                atividadesVelhas.Add(work);
                                break;
                            default:
                                throw new Exception("what");
                        }
                    }
                }
            }

            return atividadesVelhas;
        }

        private static void EscreverNoArquivo(string filePath, List<Activity> allAtividadesCategoria, Category category)
        {
            using (var file = new StreamWriter(filePath))
            {
                foreach (Activity atividade in allAtividadesCategoria)
                {
                    string diaMes = atividade.Dia.Day.ToString("00") + "/" + atividade.Dia.Month.ToString("00");

                    string consolidatedActivity;

                    switch (category)
                    {
                        case Category.Auto:
                            Auto auto = (Auto) atividade;
                            consolidatedActivity = auto.ConsolidateActivity();
                            break;
                        case Category.Doom:
                            Doom doom = (Doom)atividade;
                            consolidatedActivity = doom.ConsolidateActivity();
                            break;
                        case Category.Gift:
                            Gift gift = (Gift)atividade;
                            consolidatedActivity = gift.ConsolidateActivity();
                            break;
                        case Category.Health:
                            Health health = (Health)atividade;
                            consolidatedActivity = health.ConsolidateActivity();
                            break;
                        case Category.Person:
                            Person person = (Person)atividade;
                            consolidatedActivity = person.ConsolidateActivity();
                            break;
                        case Category.Pet:
                            Pet pet = (Pet)atividade;
                            consolidatedActivity = pet.ConsolidateActivity();
                            break;
                        case Category.Play:
                            Play play = (Play)atividade;
                            consolidatedActivity = play.ConsolidateActivity();
                            break;
                        case Category.Purchase:
                            Purchase purchase = (Purchase)atividade;
                            consolidatedActivity = purchase.ConsolidateActivity();
                            break;
                        case Category.Travel:
                            Travel travel = (Travel)atividade;
                            consolidatedActivity = travel.ConsolidateActivity();
                            break;
                        case Category.Work:
                            Work work = (Work)atividade;
                            consolidatedActivity = work.ConsolidateActivity();
                            break;
                        default:
                            throw new Exception("what");
                    }

                    file.WriteLine(consolidatedActivity);
                }
            }
        }

        protected abstract void ParseAtividadeVelha(string[] segmentos);

        protected abstract string ConsolidateActivity();
    }
}
