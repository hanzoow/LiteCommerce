using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LiteCommerce.DomainModels;

namespace LiteCommerce.Admin.Models
{
    public class OrderPagingationResult : PaginationResult
    {
        public List<Order> Data;

        public string country;

    }
}