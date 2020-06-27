﻿using LiteCommerce.Admin.Models;
using LiteCommerce.BusinessLayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(Roles =WebUserRoles.Accountant)]
    public class ReportController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page = 1)
        {
            var model = new ReportPaginationResult
            {
                Page = page,
                PageSize = AppSettings.DefaultPageSize,
                sumOfOrders = SaleManagementBLL.Order_Count("",""),
                DataSum = ReportBLL.Report_TimeOrdered(),
                DataShipOk = ReportBLL.Report_ListShipOk()
            };
            return View(model);
        }
    }
}