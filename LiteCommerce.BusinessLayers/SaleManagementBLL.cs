using LiteCommerce.DataLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    public static class SaleManagementBLL
    {
        private static IOrderDAL OrderDB { get; set; }
        public static void Initialize(string connectionString)
        {
            OrderDB = new DataLayers.SqlServer.OrderDAL(connectionString);
        }
        public static List<Order> Order_List(int page, int pageSize, string searchValue)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 1)
                pageSize = 1;
            return OrderDB.List(page, pageSize, searchValue);
        }
        public static int Order_Count(string searchValue)
        {
            return OrderDB.Count(searchValue);
        }
        public static int Order_Add(Order data)
        {
            return OrderDB.Add(data);
        }
        public static Order Order_Get(string orderID)
        {
            return OrderDB.Get(orderID);
        }
        public static bool Order_Update(Order data)
        {
            return OrderDB.Update(data);
        }
        public static bool Order_Delete(int[] orderIds)
        {
            return OrderDB.Delete(orderIds);
        }

        public static List<OrderDetails> Order_GetDetailsList(string OrderId)
        {
            return OrderDB.GetOrderDetails(OrderId);
        }
        public static bool Order_UpdateOrderDetails(string orderID,string[] productIds,string[] unitPrice, string[] quantity,string[] discount)
        {
            return OrderDB.UpdateOrderDetails(orderID,productIds, unitPrice, quantity, discount);
        }
    }
}
