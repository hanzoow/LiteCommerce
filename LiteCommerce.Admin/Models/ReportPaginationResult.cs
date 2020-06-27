using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LiteCommerce.DomainModels;

namespace LiteCommerce.Admin.Models
{
    public class ReportPaginationResult : PaginationResult
    {
        public int sumOfOrders;
        public List<Report> DataSum;
        public List<Report> DataShipOk;
    }
}