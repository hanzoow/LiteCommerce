using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using static LiteCommerce.Admin.WebUserPrincipal;

namespace LiteCommerce.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [AuthRole(Roles = WebUserRoles.Saleman)]
    public class OrderController : Controller
    {
        /// <summary>
        /// Danh sách đơn hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page = 1, string searchValue = "",string country = "")
        {
            var model = new Models.OrderPagingationResult()
            {
                Page = page,
                PageSize = AppSettings.DefaultPageSize,
                RowCount = SaleManagementBLL.Order_Count(searchValue,country),
                Data = SaleManagementBLL.Order_List(page, AppSettings.DefaultPageSize, searchValue,country),
                SearchValue = searchValue,
            };
            return View(model);
        }
        /// <summary>
        /// Tạo đơn hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.Title = "Create Order";
            ViewBag.ConfirmButton = "Create";
            Order newOrder = new Order();
            newOrder.OrderID = 0;
            return View(newOrder);
        }
        [HttpPost]
        public ActionResult Create(Order model)
        {
            try
            {
                DateTime orderDate = Convert.ToDateTime(model.OrderDate);
                DateTime requiredDate = Convert.ToDateTime(model.RequiredDate);
                //Validation dữ liệu                
                if (string.IsNullOrEmpty(model.OrderDate))
                {
                    model.OrderDate = "";
                }
                if (string.IsNullOrEmpty(model.RequiredDate))
                {
                    model.RequiredDate = "";
                }
                if (string.IsNullOrEmpty(model.ShippedDate))
                {
                    model.ShippedDate = null;
                }
                if (orderDate >= requiredDate)
                {
                    ModelState.AddModelError("Wrong", "Ngày yêu cầu giao hàng phải muộn hơn ngày đặt hàng!");
                    ViewBag.ConfirmButton = "Create";
                    return View(model);
                }
                //Kiểm tra có tồn tại bất kỳ lỗi nào hay không

                //Đưa dữ liệu vào CSDL
                int orderID = SaleManagementBLL.Order_Add(model);
                return RedirectToAction("Input/" + orderID);
            }
            catch (Exception e)
            {
                Console.WriteLine("", e.Message + ":" + e.StackTrace);
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Add New Order";
                ViewBag.ConfirmButton = "Add";
                Order newOrder = new Order();
                newOrder.OrderID = 0;
                return View(newOrder);
            }
            else
            {
                ViewBag.Title = "Edit Order";
                ViewBag.ConfirmButton = "Save";
                ViewBag.ShippedDate = "";
                try
                {
                    Order editOrder = SaleManagementBLL.Order_Get(id);
                    if (editOrder == null)
                    {
                        return RedirectToAction("Index");
                    }
                    return View(editOrder);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpPost]
        public ActionResult Input(Order model)
        {
            try
            {
                DateTime orderDate =Convert.ToDateTime(model.OrderDate);
                DateTime requiredDate = Convert.ToDateTime(model.RequiredDate);
                DateTime shippedDate = Convert.ToDateTime(model.ShippedDate);
                //Validation dữ liệu                
                if (string.IsNullOrEmpty(model.OrderDate))
                {
                    model.OrderDate = "";
                }
                if (string.IsNullOrEmpty(model.RequiredDate))
                {
                    model.RequiredDate = "";
                }
                if (orderDate >= requiredDate)
                {
                    ModelState.AddModelError("Wrong", "Ngày yêu cầu giao hàng phải muộn hơn ngày đặt hàng ít nhất 1 ngày!");
                    ViewBag.ConfirmButton = "Create";
                    return View(model);
                }
                if(shippedDate < requiredDate)
                {
                    ModelState.AddModelError("WrongShipDate", "Ngày giao hàng phải muộn hơn ngày đặt hàng ít nhất 1 ngày!");
                    ViewBag.ConfirmButton = "Save";
                    return View(model);
                }
                //Kiểm tra có tồn tại bất kỳ lỗi nào hay không
                if (!ModelState.IsValid)
                {
                    if (model.OrderID == 0)
                    {
                        ViewBag.Title = "Add New Order";
                        ViewBag.ConfirmButton = "Add";
                    }
                    else
                    {
                        ViewBag.Title = "Edit New Order";
                        ViewBag.ConfirmButton = "Save";
                    }
                    return View(model);
                }
                //Đưa dữ liệu vào CSDL
                if (model.OrderID == 0)
                {
                    int orderID = SaleManagementBLL.Order_Add(model);
                    return RedirectToAction("Input/" + orderID);
                }
                else
                {
                    bool rs = SaleManagementBLL.Order_Update(model);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("", e.Message + ":" + e.StackTrace);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Delete(int[] orderIds = null)
        {
            ViewBag.Delete = false;
            bool rs = false;
            if (orderIds != null)
            {
                rs = SaleManagementBLL.Order_Delete(orderIds);
                if(rs == true)
                {
                    TempData["orderError"] = "<script>alert('Xóa order thành công!');</script>";
                    return RedirectToAction("Index");

                }
                else
                {
                    TempData["orderError"] = "<script>alert('Xóa không thành công !');</script>";
                    return RedirectToAction("Index");

                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult Detail(string id = "")
        {
            ViewBag.Title = "Order Detail";
            if(string.IsNullOrEmpty(id))
            {
                RedirectToAction("Index");
            }
            try
            {
                Order order = SaleManagementBLL.Order_Get(id);
                return View(order);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public ActionResult ajaxOrderHehe(string id = "")
        {
            try
            {
                Order checkOrder = SaleManagementBLL.Order_Get(id);
                if (checkOrder == null)
                {
                    return RedirectToAction("Index");
                }
                var model = new Models.OrderDetailsResult
                {
                    Data = SaleManagementBLL.Order_GetDetailsList(id),
                };
                return Json(new { model.Data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public ActionResult OrderDetail(string id = "")
        {
            try
            {
                Order checkOrder = SaleManagementBLL.Order_Get(id);
                if(checkOrder == null)
                {
                    return RedirectToAction("Index");
                }
                var model = new Models.OrderDetailsResult
                {
                    Data = SaleManagementBLL.Order_GetDetailsList(id),
                    OrderID = Convert.ToInt32(id),
                };
                return View(model);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult OrderDetail(string[] UnitPrice,string[] ProductIDs, string[] Quantity, string[] Discount, string OrderID)
        {
            List<string> newProductID = new List<string>();
            List<string> newUnitPrice = new List<string>();
            List<string> newQuantity = new List<string>();
            List<string> newDiscount = new List<string>();
            for (int i = 0; i < UnitPrice.Length; i++)
            {
                if (string.IsNullOrEmpty(UnitPrice[i]) || string.IsNullOrEmpty(UnitPrice[i]))
                {
                    continue;
                }
                else
                {
                    newProductID.Add(ProductIDs[i]);
                    newUnitPrice.Add(UnitPrice[i]);
                    newQuantity.Add(Quantity[i]);
                    newDiscount.Add(Discount[i]);
                }
            }
            string[] arrayProductIds = newProductID.ToArray();
            string[] arrayUnitPrice = newUnitPrice.ToArray();
            string[] arrayQuantity = newQuantity.ToArray();
            string[] arrayDiscount = newDiscount.ToArray();

            bool rs = SaleManagementBLL.Order_UpdateOrderDetails(OrderID, arrayProductIds ,arrayUnitPrice, arrayQuantity, arrayDiscount);

            return RedirectToAction("Detail", new { id = OrderID });
        }
    }
}