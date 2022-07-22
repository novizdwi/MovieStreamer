using Microsoft.AspNetCore.Mvc;
using MovieStreamer.Models;
using MovieStreamer.Services;
using MovieStreamer.ViewModels;
using PagedList;
using Microsoft.AspNetCore.Authorization;

namespace MovieStreamer.Controllers
{
    public class MovieController : Controller
    {
        protected IHttpContextAccessor contextAccessor;
        protected ApplicationDbContext context;
        private MovieService services;
        public MovieController(IHttpContextAccessor _contextAccessor,
            ApplicationDbContext _context,
            MovieService services)
        {
            contextAccessor = _contextAccessor;
            context = _context;
            this.services = services;
        }

        public IActionResult Index(string field, string searchText, int? page)
        {
            int pageNumber = (page ?? 1);
            int recordPerPage = 10;
            int totalPage = services.CountTotalPage(recordPerPage);
            ViewBag.PageName = "Movie Lists";
            List<MovieViewModels> viewModel = services.GetAll(field, searchText, pageNumber, recordPerPage);
            return View(viewModel.ToPagedList(pageNumber, recordPerPage));
        }
    }
}
