using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication44.Models
{
    public class Offer
    {
        public int OfferId { get; set; }

        public string UserID { get; set; }

        public int LocationID { get; set; }

        public int CategoryID { get; set; }

        public string Title { get; set; }

        public string Descritpion { get; set; }

        public double Price { get; set; }

        public string Address { get; set; }

        public int Phone { get; set; }

        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        public virtual Category Category { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}