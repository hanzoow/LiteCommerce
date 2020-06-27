using LiteCommerce.DataLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    public class ReportBLL
    {
        private static IReportDAL ReportDB { get; set; }
        public static void Initialize(string connectionString)
        {
            ReportDB = new DataLayers.SqlServer.ReportDAL(connectionString);
        }

        public static List<Report> Report_TimeOrdered()
        {
            return ReportDB.GetTimeOrdered();
        }

        public static List<Report> Report_ListShipOk()
        {
            return ReportDB.GetListOrderShipOk();
        }

        public static int Report_SumPriceProduct()
        {
            return ReportDB.SumPriceProduct();
        }
    }
}
