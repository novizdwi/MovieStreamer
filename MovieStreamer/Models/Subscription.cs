using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieStreamer.Models
{
    [Table("Subcription")]
    public class Subscription: BaseClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SubscriptionType { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public string PaymentMethod { get; set; }
        public int Price { get; set; }
        public DateTime ExpiredDate { get; set; }


        [ForeignKey("UserId")]
        public virtual Users Users { get; set; }

    }
    public class SubscriptionType
    {
        public const string M = "Monthly";
        public const string Y = "Yearly";
    }
    public class PaymentMethod
    {
        public const string VA = "Virtual Account";
        public const string DBT = "Direct Bank Transfer";
        public const string MC = "Master Card";

    }
}
