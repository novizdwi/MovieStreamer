using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieStreamer.Models
{
    [Table("Users")]
    public class Users: BaseClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public string? Username { get; set; }
        public string? Email {get;set;}
        public string? Phone { get; set; }
        public string? Password { get; set; }

    }
    enum genderEnum 
    { 
        M,
        F
    }
}
