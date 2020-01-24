using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication44.Models
{
    public class Book
    {
        public int BookID { get; set; }

        public string Title { get; set; }


        public virtual ICollection<Borrow> Borrows { get; set; }
    }
}