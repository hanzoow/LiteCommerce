using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface IReportDAL
    {
        List<Report> GetTimeOrdered();
        List<Report> GetListOrderShipOk();

        int SumPriceProduct(); 
    }
}
