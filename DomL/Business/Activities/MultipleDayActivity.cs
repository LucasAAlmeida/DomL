﻿using DomL.Business.Activities.MultipleDayActivities;
using DomL.Business.Utils;
using DomL.Business.Utils.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DomL.Business.Activities
{
    public abstract class MultipleDayActivity : Activity
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
                        var segmentos = Regex.Split(line, "\t");

                        Activity atividadeVelhaComeco = Util.GetAtividadeVelha(segmentos[0], year, category, Classification.Comeco);
                        Activity atividadeVelhaTermino = Util.GetAtividadeVelha(segmentos[1], year, category, Classification.Termino);

                        switch (category)
                        {
                            case Category.Book:
                                Book book;
                                if (atividadeVelhaComeco != null)
                                {
                                    book = (Book)atividadeVelhaComeco;
                                    book.ParseAtividadeVelha(segmentos);
                                    atividadesVelhas.Add(book);
                                }
                                if (atividadeVelhaTermino != null)
                                {
                                    book = (Book)atividadeVelhaTermino;
                                    book.ParseAtividadeVelha(segmentos);
                                    atividadesVelhas.Add(book);
                                }
                                break;
                            case Category.Comic:
                                Comic comic;
                                if (atividadeVelhaComeco != null)
                                {
                                    comic = (Comic)atividadeVelhaComeco;
                                    comic.ParseAtividadeVelha(segmentos);
                                    atividadesVelhas.Add(comic);
                                }
                                if (atividadeVelhaTermino != null)
                                {
                                    comic = (Comic)atividadeVelhaTermino;
                                    comic.ParseAtividadeVelha(segmentos);
                                    atividadesVelhas.Add(comic);
                                }
                                break;
                            case Category.Game:
                                Game game;
                                if (atividadeVelhaComeco != null)
                                {
                                    game = (Game)atividadeVelhaComeco;
                                    game.ParseAtividadeVelha(segmentos);
                                    atividadesVelhas.Add(game);
                                }
                                if (atividadeVelhaTermino != null)
                                {
                                    game = (Game)atividadeVelhaTermino;
                                    game.ParseAtividadeVelha(segmentos);
                                    atividadesVelhas.Add(game);
                                }
                                break;
                            case Category.Movie:
                                Movie movie;
                                if (atividadeVelhaComeco != null)
                                {
                                    movie = (Movie)atividadeVelhaComeco;
                                    movie.ParseAtividadeVelha(segmentos);
                                    atividadesVelhas.Add(movie);
                                }
                                if (atividadeVelhaTermino != null)
                                {
                                    movie = (Movie)atividadeVelhaTermino;
                                    movie.ParseAtividadeVelha(segmentos);
                                    atividadesVelhas.Add(movie);
                                }
                                break;
                            case Category.Series:
                                Series series;
                                if (atividadeVelhaComeco != null)
                                {
                                    series = (Series)atividadeVelhaComeco;
                                    series.ParseAtividadeVelha(segmentos);
                                    atividadesVelhas.Add(series);
                                }
                                if (atividadeVelhaTermino != null)
                                {
                                    series = (Series)atividadeVelhaTermino;
                                    series.ParseAtividadeVelha(segmentos);
                                    atividadesVelhas.Add(series);
                                }
                                break;
                            case Category.Watch:
                                Watch watch;
                                if (atividadeVelhaComeco != null)
                                {
                                    watch = (Watch)atividadeVelhaComeco;
                                    watch.ParseAtividadeVelha(segmentos);
                                    atividadesVelhas.Add(watch);
                                }
                                if (atividadeVelhaTermino != null)
                                {
                                    watch = (Watch)atividadeVelhaTermino;
                                    watch.ParseAtividadeVelha(segmentos);
                                    atividadesVelhas.Add(watch);
                                }
                                break;
                            default:
                                throw new Exception("what");
                        }
                    }
                }
            }

            return atividadesVelhas;
        }

        private static void EscreverNoArquivo(string filePath, List<Activity> allCategoryActivities, Category category)
        {
            using (var file = new StreamWriter(filePath))
            {
                foreach (Activity activity in allCategoryActivities)
                {
                    string dataInicio = "??/??";
                    string dataTermino = "??/??";

                    switch (activity.Classificacao)
                    {
                        case Classification.Unica:
                            dataInicio = activity.Dia.Day.ToString("00") + "/" + activity.Dia.Month.ToString("00");
                            dataTermino = dataInicio;
                            break;

                        case Classification.Comeco:
                            dataInicio = activity.Dia.Day.ToString("00") + "/" + activity.Dia.Month.ToString("00");

                            Activity atividadeTermino = allCategoryActivities.FirstOrDefault(a => a.Classificacao == Classification.Termino && Util.IsEqualTitle(a.Assunto, activity.Assunto));
                            if (atividadeTermino != null)
                            {
                                dataTermino = atividadeTermino.Dia.Day.ToString("00") + "/" + atividadeTermino.Dia.Month.ToString("00");
                                activity.Valor = atividadeTermino.Valor;
                                activity.Descricao = string.IsNullOrWhiteSpace(activity.Descricao) ? atividadeTermino.Descricao : activity.Descricao + ", " + atividadeTermino.Descricao;
                            }
                            break;

                        case Classification.Termino:
                            //Pra não fazer duas vezes a mesma atividade
                            Activity atividadeComeco = allCategoryActivities.FirstOrDefault(a => a.Classificacao == Classification.Comeco && Util.IsEqualTitle(a.Assunto, activity.Assunto));
                            if (atividadeComeco != null)
                            {
                                continue;
                            }

                            dataTermino = activity.Dia.Day.ToString("00") + "/" + activity.Dia.Month.ToString("00");
                            break;

                        case Classification.Indefinido:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    switch (category)
                    {
                        case Category.Book:
                            Book book = (Book)activity;
                            book.WriteAtividadeConsolidada(file, dataInicio, dataTermino);
                            break;
                        case Category.Comic:
                            Comic comic = (Comic)activity;
                            comic.WriteAtividadeConsolidada(file, dataInicio, dataTermino);
                            break;
                        case Category.Game:
                            Game game = (Game)activity;
                            game.WriteAtividadeConsolidada(file, dataInicio, dataTermino);
                            break;
                        case Category.Movie:
                            Movie movie = (Movie)activity;
                            movie.WriteAtividadeConsolidada(file, dataInicio, dataTermino);
                            break;
                        case Category.Series:
                            Series series = (Series)activity;
                            series.WriteAtividadeConsolidada(file, dataInicio, dataTermino);
                            break;
                        case Category.Watch:
                            Watch watch = (Watch)activity;
                            watch.WriteAtividadeConsolidada(file, dataInicio, dataTermino);
                            break;
                        default:
                            throw new Exception("what");
                    }
                }
            }

        }

        protected abstract void ParseAtividadeVelha(string[] segmentos);

        protected abstract void WriteAtividadeConsolidada(StreamWriter file, string dataInicio, string dataTermino);
    }
}