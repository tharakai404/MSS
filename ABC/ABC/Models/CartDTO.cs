using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABC.Models
{
    public class CartDTO

    {
        public int id { get; set; }
        public string item { get; set; }
        public int qty { get; set; }
        public Boolean Wash { get; set; }
        public Boolean Iron { get; set; }
        public Boolean Dryclean { get; set; }
        public decimal value { get; set; }

    }
}