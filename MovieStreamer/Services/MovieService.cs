using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using MovieStreamer.Models;
using MovieStreamer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieStreamer.Services
{
    public class MovieService
    {
        private readonly ApplicationDbContext db;
        private readonly IConfiguration _configuration;
        public MovieService(ApplicationDbContext context)
        {
            db = context;
        }
        public int CountTotalPage(int recordPerPage = 10)
        {
            var ret = db.Movies.Count();
            return ret / recordPerPage;
        }

        internal List<MovieViewModels> GetAll(string field, string searchText, int page, int recordPerPage)
        {
            var ret =(from m in db.Movies
                      select new MovieViewModels() {
                          Id = m.Id,
                          Title = m.Title,
                          Description = m.Description,
                          ImageUrl = m.ImageUrl,
                          Episodes = m.Episodes,
                          ReleaseDate = m.ReleaseDate,
                          ReleaseYear = m.ReleaseYear,
                          MovieDuration = m.MovieDuration,
                          IsSubcribe = 0,
                      });

            ret.Skip((page - 1) * recordPerPage).Take(recordPerPage);
            return ret.ToList();
        }
    }
}
