using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication44.Models
{
    public class Borrow
    {
        public int BorrowID { get; set; }

        public int BookID { get; set; }

        public string UserID { get; set; }


        public virtual Book Book { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}