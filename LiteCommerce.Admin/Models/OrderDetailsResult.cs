using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LiteCommerce.DomainModels;

namespace LiteCommerce.Admin.Models
{
    public class OrderDetailsResult
    {
        public List<OrderDetails> Data;

        public int OrderID;

        public Product Product;
    }
}