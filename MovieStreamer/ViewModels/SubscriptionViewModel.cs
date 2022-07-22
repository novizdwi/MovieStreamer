using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MovieStreamer.ViewModels
{
    public class SubscriptionViewModel
    {
        public List<SelectListItem> SubscriptionTypeList{ get; set; }
        public List<SelectListItem> PaymentMethodList{ get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required]
        [Display(Name ="Subscription Type")]
        public string SubscriptionType { get; set; }
        public string SubscriptionTypeText { get; set; }
        public DateTime SubscriptionDate { get; set; }
        [Required] 
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }
        public string PaymentMethodText { get; set; }
        public int Price { get; set; }
        [Display(Name = "Valid Until")]
        public DateTime ExpiredDate { get; set; }

        internal string? ToPagedList(int pageNumber, int recordPerPage)
        {
            throw new NotImplementedException();
        }
    }
}
