using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casestudy.Models
{
    public class OrderViewModel
    {
      
        public int OrderId { get; set; }
        public string OrderDate { get; set; }
     
        public string OrderAmount { get; set; }
        public string ProductName { get; set; }

        public string ProductId { get; set; }
        public string UserId { get; set; }

        public int QtyOrdered { get; set; }
        public int QtySold { get; set; }
        public int QtyBackOrdered { get; set; }

        public string SellingPrice { get; set; }
        public string Extended { get; set; }
        public string Tax { get; set; }
        public string SubTotal { get; set; }








    }
}
