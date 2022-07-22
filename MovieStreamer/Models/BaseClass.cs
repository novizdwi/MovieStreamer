using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieStreamer.Models
{
    public class BaseClass
    {
        public DateTime? CreatedDate {get;set;}
        public string? CreatedBy {get;set;}
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }

    public class TextPair
    {
        public string Value { get; set; }
        public string Text {get; set;}
    }
}
