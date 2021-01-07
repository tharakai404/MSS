using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABC.Models
{
    public class DuePayDTO
    {
        public int id { get; set; }

        public string classname { get; set; }
        public string name { get; set; }
        public decimal amount { get; set; }

    }
}