using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;

namespace LiteCommerce.DataLayers.SqlServer
{
    /// <summary>
    /// 
    /// </summary>
    public class OrderDAL : IOrderDAL
    {
        /// <summary>
        /// 
        /// </summary>
        private string connectionString;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public OrderDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Order data)
        {
            int orderId = 0;

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Orders
                                          (
                                                CustomerID,
                                                EmployeeID,
                                                OrderDate,
                                                RequiredDate,
                                                ShippedDate,
                                                ShipperID,
                                                Freight,
                                                ShipAddress,
                                                ShipCity,
                                                ShipCountry
                                          )
                                          VALUES
                                          (
                                                @CustomerID,
                                                @EmployeeID,
                                                @OrderDate,
                                                @RequiredDate,
                                                @ShippedDate,
                                                @ShipperID,
                                                @Freight,
                                                @ShipAddress,
                                                @ShipCity,
                                                @ShipCountry
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CustomerID", data.CustomerID);
                cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);
                cmd.Parameters.AddWithValue("@OrderDate", Convert.ToDateTime(data.OrderDate));
                cmd.Parameters.AddWithValue("@RequiredDate", Convert.ToDateTime(data.RequiredDate));
                cmd.Parameters.AddWithValue("@ShippedDate", data.ShippedDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ShipperID", data.ShipperID);
                cmd.Parameters.AddWithValue("@Freight", data.Freight);
                cmd.Parameters.AddWithValue("@ShipAddress", data.ShipAddress);
                cmd.Parameters.AddWithValue("@ShipCity", data.ShipCity);
                cmd.Parameters.AddWithValue("@ShipCountry", data.ShipCountry);

                orderId = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }

            return orderId;
        }

        public int Count(string searchValue,string country)
        {
            int dem;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT count(*)
                                        FROM Orders
                                        WHERE(
                                                (@searchValue = N'')
                                                OR (ShipAddress like @searchValue)
                                            )
                                            AND(
                                                (@country = N'')
                                                OR (ShipCountry = @country)
                                            )";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    cmd.Parameters.AddWithValue("@country", country);
                    dem = Convert.ToInt32(cmd.ExecuteScalar());
                }
                connection.Close();
            }
            return dem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderIDs"></param>
        /// <returns></returns>
        public bool Delete(int[] orderIDs)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Orders
                                            WHERE(OrderID = @orderID)
                                              AND(OrderID NOT IN(SELECT OrderID FROM OrderDetails))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@OrderID", SqlDbType.NVarChar);
                foreach (int orderId in orderIDs)
                {
                    cmd.Parameters["@orderID"].Value = orderId;
                    result += cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
            return result > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public Order Get(string orderID)
        {
            Order data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT *
                                    FROM Orders
                                    WHERE OrderID = @orderID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@orderID", orderID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Order()
                        {
                            OrderID = Convert.ToInt32(dbReader["OrderID"]),
                            CustomerID = Convert.ToString(dbReader["CustomerID"]),
                            EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                            OrderDate = Convert.ToString(dbReader["OrderDate"]),
                            RequiredDate = Convert.ToString(dbReader["RequiredDate"]),
                            ShippedDate = Convert.ToString(dbReader["ShippedDate"]),
                            ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                            Freight = Convert.ToString(dbReader["Freight"]),
                            ShipAddress = Convert.ToString(dbReader["ShipAddress"]),
                            ShipCity = Convert.ToString(dbReader["ShipCity"]),
                            ShipCountry = Convert.ToString(dbReader["ShipCountry"])
                        };
                    }
                }
                connection.Close();
            }
            return data;
        }

        public List<OrderDetails> GetOrderDetails(string orderID)
        {
            List<OrderDetails> data = new List<OrderDetails>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT * FROM OrderDetails WHERE OrderID = @orderID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@orderID", Convert.ToInt32(orderID));
                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new OrderDetails()
                            {
                                OrderID = Convert.ToInt32(dbReader["OrderID"]),
                                ProductID = Convert.ToInt32(dbReader["ProductID"]),
                                UnitPrice = Convert.ToInt32(dbReader["UnitPrice"]),
                                Quantity = Convert.ToInt32(dbReader["Quantity"]),
                                Discount = Convert.ToInt32(dbReader["Discount"]),
                            });
                        }
                    }
                }
                connection.Close();
            }
            return data;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public List<Order> List(int page, int pageSize, string searchValue,string country)
        {
            List<Order> data = new List<Order>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT *
                                        FROM(
                                            SELECT *, ROW_NUMBER() OVER(ORDER BY OrderID) AS RowNumber
                                            FROM Orders
                                            WHERE(
				                                    (@searchValue = N'')
				                                    OR (ShipAddress like @searchValue)
				                                    )
                                                AND (
                                                    (@country = N'')
				                                    OR (ShipCountry = @country)
                                                    )
                                        ) AS t WHERE t.RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize
                                        ORDER BY t.RowNumber";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@page", page);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    cmd.Parameters.AddWithValue("@country", country);
                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new Order()
                            {
                                OrderID = Convert.ToInt32(dbReader["OrderID"]),
                                CustomerID = Convert.ToString(dbReader["CustomerID"]),
                                EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                                OrderDate = Convert.ToString(dbReader["OrderDate"]),
                                RequiredDate = Convert.ToString(dbReader["RequiredDate"]),
                                ShippedDate = Convert.ToString(dbReader["ShippedDate"]),
                                ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                                Freight = Convert.ToString(dbReader["Freight"]),
                                ShipAddress = Convert.ToString(dbReader["ShipAddress"]),
                                ShipCity = Convert.ToString(dbReader["ShipCity"]),
                                ShipCountry = Convert.ToString(dbReader["ShipCountry"])
                            });
                        }
                    }
                }
                connection.Close();
            }
            return data;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(Order data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Orders
                                    SET                                    
                                        CustomerID = @CustomerID,
                                        EmployeeID = @EmployeeID,
                                        OrderDate = @OrderDate,
                                        RequiredDate = @RequiredDate,
                                        ShippedDate = @ShippedDate,
                                        ShipperID = @ShipperID,
                                        Freight = @Freight,
                                        ShipAddress = @ShipAddress,
                                        ShipCity = @ShipCity,
                                        ShipCountry = @ShipCountry
                                    WHERE OrderID = @OrderID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@OrderID", data.OrderID);
                cmd.Parameters.AddWithValue("@CustomerID", data.CustomerID);
                cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);
                cmd.Parameters.AddWithValue("@OrderDate", Convert.ToDateTime(data.OrderDate));
                cmd.Parameters.AddWithValue("@RequiredDate", Convert.ToDateTime(data.RequiredDate));
                cmd.Parameters.AddWithValue("@ShippedDate", Convert.ToDateTime(data.ShippedDate));
                cmd.Parameters.AddWithValue("@ShipperID", data.ShipperID);
                cmd.Parameters.AddWithValue("@Freight", data.Freight);
                cmd.Parameters.AddWithValue("@ShipAddress", data.ShipAddress);
                cmd.Parameters.AddWithValue("@ShipCity", data.ShipCity);
                cmd.Parameters.AddWithValue("@ShipCountry", data.ShipCountry);
                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());
                connection.Close();
            }

            return rowsAffected > 0;
        }

        public bool UpdateOrderDetails(string orderID,string[] productIDs, string[] unitPrice, string[] quantity, string[] discount)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM OrderDetails WHERE OrderID = @OrderID AND ProductID = @ProductID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@orderID",orderID);
                for(int j = 0; j < unitPrice.Length; j++)
                {
                    cmd.Parameters["@ProductID"].Value = Convert.ToInt32(productIDs[j]);
                    result += cmd.ExecuteNonQuery();
                }

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = @"INSERT INTO OrderDetails (OrderID,ProductID,UnitPrice,Quantity,Discount) VALUES (@OrderID,@ProductID,@UnitPrice,@Quantity,@Discount)";
                cmd2.CommandType = CommandType.Text;
                cmd2.Connection = connection;
                cmd2.Parameters.AddWithValue("@OrderID", orderID);
                cmd2.Parameters.Add("@ProductID", SqlDbType.Int);
                cmd2.Parameters.Add("@UnitPrice", SqlDbType.Money);
                cmd2.Parameters.Add("@Quantity", SqlDbType.SmallInt);
                cmd2.Parameters.Add("@Discount", SqlDbType.Real);
                for (int i = 0; i < unitPrice.Length; i++)
                {
                    cmd2.Parameters["@ProductID"].Value = productIDs[i];
                    cmd2.Parameters["@UnitPrice"].Value = unitPrice[i];
                    cmd2.Parameters["@Quantity"].Value = quantity[i];
                    cmd2.Parameters["@Discount"].Value = (discount[i]) == null ? 0 : Convert.ToInt32(discount[i]);
                    result += cmd2.ExecuteNonQuery();
                }
                connection.Close();
            }
            return result > 0;
        }
    }
}
