using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieStreamer.Models;

namespace MovieStreamer.ViewModels
{
    public class MovieViewModels
    {
        public List<Movie> MovieList { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public int? Episodes { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? ReleaseYear { get; set; }
        public string? MovieDuration { get; set; }
        public int IsSubcribe { get; set; }
    }
}
