using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LiteCommerce.Admin.Models;
using LiteCommerce.BusinessLayers;

namespace LiteCommerce.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class DashboardController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new DashboardResult
            {
                sumSupplier = CatalogBLL.Supplier_Count(""),
                sumCustomer = CatalogBLL.Customer_Count(""),
                sumShipper = CatalogBLL.Shipper_Count(""),
                sumCategory = CatalogBLL.Category_Count(""),
                sumProduct = CatalogBLL.Product_Count("", "", ""),
                sumOrder = SaleManagementBLL.Order_Count("", ""),
                sumEmployee = HumanResourceBLL.Employee_Count("", "")
            };
            return View(model);
        }
    }
}