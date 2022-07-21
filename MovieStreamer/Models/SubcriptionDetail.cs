using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieStreamer.Models
{
    [Table("SubcriptionDetail")]
    public class SubcriptionDetail : BaseClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SubcriptionId { get; set; }
        public int MovieId { get; set; }
        public int price { get; set; }

    }
}
