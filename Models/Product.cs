using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompoShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Producer { get; set; }
        public string Processor { get; set; }
        public string RAM { get; set; }
        public string HDD { get; set; }
        public string Body { get; set; }
        public byte[] Picture { get; set; }
        public int Price { get; set; }
    }
}