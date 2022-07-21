using System;
using System.Collections.Generic;
using System.Text;
using MovieStreamer.Models;

namespace MovieStreamer.Services
{
    public class BaseService
    {
        protected ApplicationDbContext db;
        public BaseService() : base()
        {
            db = new ApplicationDbContext();
        }

    }
}
