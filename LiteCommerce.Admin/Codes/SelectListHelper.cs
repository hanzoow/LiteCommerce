using LiteCommerce.BusinessLayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin
{
    public class SelectListHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string[] ListOfRoles()
        {
            string[] roles = { "Accounttant", "Saleman", "Administrator" };
            return roles;
        }
        public static List<SelectListItem> ListOfProducts(bool allowSelectAll = true)
        {
            List<SelectListItem> listProducts = new List<SelectListItem>();
            if (allowSelectAll)
            {
                listProducts.Add(new SelectListItem() { Value = "", Text = "--- All Employee ---" });
            }
            foreach (var item in CatalogBLL.Product_List_Helper())
            {
                listProducts.Add(new SelectListItem() { Value = Convert.ToString(item.ProductID), Text = item.ProductName });
            }
            return listProducts;
        }
        public static List<SelectListItem> ListOfEmployees(bool allowSelectAll = true)
        {
            List<SelectListItem> listEmployees = new List<SelectListItem>();
            if (allowSelectAll)
            {
                listEmployees.Add(new SelectListItem() { Value = "", Text = "--- All Employee ---" });
            }
            foreach (var item in HumanResourceBLL.Employee_List_Helper())
            {
                listEmployees.Add(new SelectListItem() { Value = Convert.ToString(item.EmployeeID), Text = item.LastName });
            }
            return listEmployees;
        }
        public static List<SelectListItem> ListOfCustomers(bool allowSelectAll = true)
        {
            List<SelectListItem> listCustomers = new List<SelectListItem>();
            if (allowSelectAll)
            {
                listCustomers.Add(new SelectListItem() { Value = "", Text = "--- All Customer ---" });
            }
            foreach (var item in CatalogBLL.Customer_List_Helper())
            {
                listCustomers.Add(new SelectListItem() { Value = Convert.ToString(item.CustomerID), Text = item.ContactName });
            }
            return listCustomers;
        }
        public static List<SelectListItem> ListOfShippers(bool allowSelectAll = true)
        {
            List <SelectListItem> listShippers = new List<SelectListItem>();
            if (allowSelectAll)
            {
                listShippers.Add(new SelectListItem() { Value = "", Text = "--- All Shippers ---" });
            }
            foreach (var item in CatalogBLL.Shipper_List(""))
            {
                listShippers.Add(new SelectListItem() { Value = Convert.ToString(item.ShipperID), Text = item.CompanyName });
            }
            return listShippers;
        }
        public static List<SelectListItem> ListOfCountries(bool allowSelectAll = true) {
            List<SelectListItem> listCountries = new List<SelectListItem>();
            if (allowSelectAll)
            {
                listCountries.Add(new SelectListItem() { Value = "", Text = "--- All Country ---" });
            }
            foreach (var item in CatalogBLL.Country_List())
            {
                listCountries.Add(new SelectListItem() { Value = item.Country,Text= item.Country });
            }
            return listCountries;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> ListOfCategories(bool allowSelectAll = true)
        {
            List<SelectListItem> listCategory = new List<SelectListItem>();
            if (allowSelectAll)
            {
                listCategory.Add(new SelectListItem() { Value = "",Text ="--- All Category ---"});
            }
            foreach(var item in CatalogBLL.Category_List(""))
            {
                listCategory.Add(new SelectListItem() { Value = Convert.ToString(item.CategoryID), Text = item.CategoryName});
            }
            return listCategory;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> ListOfSuppliers(bool allowSelectAll = true)
        {
            List<SelectListItem> listSupplier = new List<SelectListItem>();
            if (allowSelectAll)
            {
                listSupplier.Add(new SelectListItem() { Value = "", Text = "--- All Supplier ---" });
            }
            foreach (var item in CatalogBLL.Supplier_ListNoPagination())
            {
                listSupplier.Add(new SelectListItem() { Value = Convert.ToString(item.SupplierID), Text = item.CompanyName });
            }
            return listSupplier;
        }
    }
}