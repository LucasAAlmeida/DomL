﻿using DomL.Business.Entities;

namespace DomL.DataAccess
{
    public class SeriesRepository : BaseRepository<Series>
    {
        public SeriesRepository(DomLContext context)
        : base(context)
        {
        }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }
    }
}
