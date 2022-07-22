using Microsoft.AspNetCore.Mvc;
using MovieStreamer.Models;
using MovieStreamer.Services;
using MovieStreamer.ViewModels;
using PagedList;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MovieStreamer.Controllers
{
    public class SubscriptionController : Controller
    {
        protected IHttpContextAccessor contextAccessor;
        protected ApplicationDbContext context;
        private SubscriptionService services;
        public SubscriptionController(IHttpContextAccessor _contextAccessor,
            ApplicationDbContext _context,
            SubscriptionService services)
        {
            contextAccessor = _contextAccessor;
            context = _context;
            this.services = services;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult History(int? page)
        {
            int pageNumber = (page ?? 1);
            int recordPerPage = 20;
            int userId = Convert.ToInt32(this.ClaimUserId());
            int totalPage = services.CountTotalPage(recordPerPage, userId);
            List<SubscriptionViewModel> viewModel = services.GetAll(userId, pageNumber, recordPerPage);
            return View(viewModel.ToPagedList(pageNumber, recordPerPage));
        }

        [Authorize]
        public IActionResult Subscribe()
        {
            SubscriptionViewModel viewModel = new SubscriptionViewModel();
            viewModel.SubscriptionTypeList = services.GetSubscriptionType();
            viewModel.PaymentMethodList = services.GetPaymentMethod();
            
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Subscribe(SubscriptionViewModel vm)
        {
            string msg = "";
            var success = true;
            vm.SubscriptionTypeList = services.GetSubscriptionType();
            vm.PaymentMethodList = services.GetPaymentMethod();

            if(vm.PaymentMethod == null){
                msg = "Payment Method must not null";
                success = false;
            }
            if(vm.SubscriptionType == null){
                msg = "Subscription Type must not null";
                success = false;
            }
            if (success)
            {
                vm.UserId = Convert.ToInt32(this.ClaimUserId());
                DateTime xd = DateTime.Now;
                vm.Price = vm.SubscriptionType == "M" ? 150350 : 1503700;
                vm.ExpiredDate = vm.SubscriptionType == "M" ? xd.AddMonths(1) : xd.AddYears(1);
                var result = await services.Subscribe(vm);
                if (result.Succeeded)
                {
                    return LocalRedirect("/");
                }
            }

            ViewBag.Message = msg;
            return View(vm);
        }
        private string ClaimUserId()
        {
            var identity = (ClaimsIdentity)User.Identity;
            string userId = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
            return userId;
        }
    }
}
