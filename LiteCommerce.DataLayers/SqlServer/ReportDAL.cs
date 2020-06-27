using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class ReportDAL : IReportDAL
    {
        private string connectionString;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public ReportDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Report> GetListOrderShipOk()
        {

            List<Report> data = new List<Report>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT TOP(20) OrderID ,RequiredDate,ShippedDate, CASE WHEN RequiredDate > ShippedDate THEN 'true' WHEN RequiredDate = ShippedDate THEN 'true' WHEN ShippedDate = null THEN 'true' ELSE 'false' END AS isOK FROM Orders";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new Report()
                            {
                                orderID = Convert.ToInt32(dbReader["OrderID"]),
                                requiredDate = Convert.ToString(dbReader["RequiredDate"]),
                                shippedDate = Convert.ToString(dbReader["ShippedDate"]),
                                isShipOk = Convert.ToString(dbReader["isOK"])
                            });
                        }
                    }
                }
                connection.Close();
            }
            return data;
        }

        public List<Report> GetTimeOrdered()
        {
            List<Report> data = new List<Report>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT ShipCountry as Ship,Count(ShipCountry) as sum FROM Orders GROUP BY ShipCountry ORDER BY sum";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new Report()
                            {
                                nameCountryOrder = Convert.ToString(dbReader["Ship"]),
                                sumPerCountry = Convert.ToInt32(dbReader["sum"]),
                            });
                        }
                    }
                }
                connection.Close();
            }
            return data;
        }
    }
}
