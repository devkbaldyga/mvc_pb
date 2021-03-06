﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication44.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Offer> Offers { get; set; }
    }
}