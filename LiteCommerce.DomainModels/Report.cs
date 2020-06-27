using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DomainModels
{
    public class Report
    {
        public int sumOfORders;

        public string nameCountryOrder;
        public int sumPerCountry;
        public int orderID;
        public string requiredDate;
        public string shippedDate;
        public string isShipOk;
    }
}
