using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieStreamer.Models
{
    [Table("Subcription")]
    public class Subscription
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SubscriptionType { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public DateTime ExpiredDate { get; set; }
    }

    public enum SubscriptionEnum
    {
        Gold,
        Platinum
    }
}
