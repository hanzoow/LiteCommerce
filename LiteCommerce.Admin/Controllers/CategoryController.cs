﻿using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static LiteCommerce.Admin.WebUserPrincipal;

namespace LiteCommerce.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [AuthRole(Roles = WebUserRoles.Accountant)]
    public class CategoryController : Controller
    {

        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string searchValue = "")
        {
            
            var model = new Models.CategoryResult
            {
                RowCount = CatalogBLL.Category_Count(searchValue),
                Data = CatalogBLL.Category_List(searchValue),
            };
            //var listOfSuppliers = CatalogBLL.Supplier_List(page, 10, searchValue);
            //int rowCount = CatalogBLL.Supplier_Count(searchValue);
            //ViewBag.rc = rowCount;
            return View(model);
        }
        /// <summary>
        /// Hiển thị form thêm/sửa category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Add New Category";
                ViewBag.ConfirmButton = "Add";

                Category newCategory = new Category();
                newCategory.CategoryID = 0;
                return View(newCategory);
            }
            else
            {
                ViewBag.Title = "Edit Category";
                ViewBag.ConfirmButton = "Save";
                try
                {
                    Category editCategory = CatalogBLL.Category_Get(Convert.ToInt32(id));
                    if (editCategory == null)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(editCategory);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return RedirectToAction("Index");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Input(Category model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Description))
                {
                    model.Description = "";
                }

                if (!ModelState.IsValid)
                {
                    if (model.CategoryID == 0)
                    {
                        ViewBag.Title = "Add New Shipper";
                        ViewBag.ConfirmButton = "Add";
                    }
                    else
                    {
                        ViewBag.Title = "Edit New Shipper";
                        ViewBag.ConfirmButton = "Save";
                    }
                    return View(model);
                }
                //Đưa dữ liệu vào CSDL
                if (model.CategoryID == 0)
                {
                    int shipperID = CatalogBLL.Category_Add(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    bool rs = CatalogBLL.Category_Update(model);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                return View(model);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int[] categoryIDs)
        {
            if (categoryIDs != null)
            {
                var rs = CatalogBLL.Category_Delete(categoryIDs);
                if (rs == true)
                {
                    TempData["Error"] = "<script>alert('Xóa category thành công!');</script>";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "<script>alert('Xóa không thành công !');</script>";
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Detail(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }
            try
            {
                Category model = CatalogBLL.Category_Get(Convert.ToInt32(id));
                return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }
    }
}