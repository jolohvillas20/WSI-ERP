using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSOINV.Functions
{
    public class SO_Report
    {
        public static DataTable getSOByDateRange(SqlConnection connection, string dateFrom, string dateTo)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT 
a.idSOHeader,
d.idItem,
b.User_Name, 
a.SO_Number, 
'Partner Name' = 'LSI Leading Technologies, Inc.',
CONVERT(date,a.Due_Date) as Due_Date,
d.Principal_SKU, 
d.Description, 
c.Qty, 
c.Cost, 
e.Company_Name, 
e.Address4,
'Ship to Party Country' = 'Philippines',
a.End_User,
a.End_User_City,
d.Item_Number,
a.Pick_Status,
c.Tax_Amount,
(SELECT TOP 1 aps.PO_Number FROM a_Pick_Serial as aps WHERE aps.idPickDetail = (SELECT TOP 1 apd.idPickDetail FROM a_Pick_Detail as apd WHERE apd.idItem = c.idItem AND apd.idPickHeader = (SELECT TOP 1 aph.idPickHeader FROM a_Pick_Header as aph WHERE aph.idSOHeader = a.idSOHeader ))) as PO_Number,
f.Invoice_Number,
f.Invoice_Date,
f.Del_Date,
f.OR_Number,
a.Forex_Rate,
a.credit_term
FROM a_SO_Header as a
INNER JOIN a_Users as b ON a.Salesman = b.idUser
INNER JOIN a_SO_Detail as c ON a.idSOHeader = c.idSOHeader
INNER JOIN a_Item_Master as d on c.idItem = d.idItem
INNER JOIN a_Customer_Details as e on a.idCustomer = e.idCustomer 
LEFT JOIN a_Invoice as f ON a.idSOHeader = f.idSOHeader
WHERE CONVERT(date,Due_Date) BETWEEN CONVERT(date,'" + dateFrom + "') AND CONVERT(date,'" + dateTo + "') AND Active = 'Y' ");

            DataTable dt = new DataTable();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                dt.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            connection.Close();

            return dt;
        }

        public static DataTable getCostPerSOPerItemNumber(SqlConnection connection, int idSOHeader, int idItem, int qty)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"
                        SELECT AVG(ais.Unit_Cost) FROM a_Item_Serial as ais INNER JOIN
						a_Pick_Serial as aps ON ais.idSerial = aps.idSerial
						WHERE aps.idPickDetail = 
						(
						    SELECT TOP 1 apd.idPickDetail FROM a_Pick_Detail as apd WHERE apd.idPickHeader = 
							(
								SELECT TOP 1 aph.idPickHeader FROM a_Pick_Header as aph WHERE aph.idSOHeader = @idSOHeader
							)
							AND apd.idItem = @idItem AND apd.Qty = @qty				
						)
						");

            DataTable dtUnitCost = new DataTable();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                SqlParameter pidItem = new SqlParameter
                {
                    ParameterName = "@idItem",
                    SqlDbType = SqlDbType.Int,
                    Value = idItem
                };
                cmd.Parameters.Add(pidItem);

                SqlParameter pidSOHeader = new SqlParameter
                {
                    ParameterName = "@idSOHeader",
                    SqlDbType = SqlDbType.Int,
                    Value = idSOHeader
                };
                cmd.Parameters.Add(pidSOHeader);

                SqlParameter pqty = new SqlParameter
                {
                    ParameterName = "@qty",
                    SqlDbType = SqlDbType.Int,
                    Value = qty
                };
                cmd.Parameters.Add(pqty);

                dtUnitCost.Load(cmd.ExecuteReader());

                cmd.Dispose();
            }

            connection.Close();

            return dtUnitCost;
        }

        //public static DataTable getCostPerSOPerItemNumber(SqlConnection connection, int idSOHeader, int idItem)
        //{
        //    DataTable dtUnitCost = new DataTable();

        //    StringBuilder sQuery = new StringBuilder();

        //    sQuery.Append(@"
        //                    SELECT idPickHeader 
        //                    FROM a_Pick_Header 
        //                    WHERE idSOHeader = @idSOHeader	
        //                    ");

        //    DataTable dtPickHeader = new DataTable();

        //    connection.Open();

        //    using (SqlCommand cmd = new SqlCommand())
        //    {
        //        cmd.Connection = connection;
        //        cmd.CommandText = sQuery.ToString();
        //        cmd.CommandType = CommandType.Text;

        //        SqlParameter pidSOHeader = new SqlParameter
        //        {
        //            ParameterName = "@idSOHeader",
        //            SqlDbType = SqlDbType.Int,
        //            Value = idSOHeader
        //        };
        //        cmd.Parameters.Add(pidSOHeader);

        //        var oreader = cmd.ExecuteReader();
        //        int pickHeaderID = 0;

        //        while (oreader.Read())
        //        {
        //            pickHeaderID = (int)oreader["idPickHeader"];

        //            sQuery = new StringBuilder();

        //            sQuery.Append(@"                     
        //          SELECT idPickDetail 
        //                            FROM a_Pick_Detail
        //                            WHERE idPickHeader = @idPickHeader AND idItem = @idItem		
        //                            ");

        //            using (SqlCommand cmd2 = new SqlCommand())
        //            {
        //                cmd2.Connection = connection;
        //                cmd2.CommandText = sQuery.ToString();
        //                cmd2.CommandType = CommandType.Text;

        //                SqlParameter pidItem = new SqlParameter
        //                {
        //                    ParameterName = "@idItem",
        //                    SqlDbType = SqlDbType.Int,
        //                    Value = idItem
        //                };
        //                cmd2.Parameters.Add(pidItem);

        //                SqlParameter idPickHeader = new SqlParameter
        //                {
        //                    ParameterName = "@idPickHeader",
        //                    SqlDbType = SqlDbType.Int,
        //                    Value = pickHeaderID
        //                };
        //                cmd2.Parameters.Add(idPickHeader);

        //                int idPickDetail = 0;

        //                while (oreader.Read())
        //                {
        //                    idPickDetail = (int)oreader["idPickDetail"];

        //                    sQuery = new StringBuilder();

        //                    sQuery.Append(@"
        //                                    SELECT AVG(ais.Unit_Cost) FROM a_Item_Serial as ais INNER JOIN
        //                                    a_Pick_Serial as aps ON ais.idSerial = aps.idSerial
        //                                    WHERE aps.idPickDetail = @idPickDetail					
        //                                    ");

        //                    using (SqlCommand cmd3 = new SqlCommand())
        //                    {
        //                        cmd3.Connection = connection;
        //                        cmd3.CommandText = sQuery.ToString();
        //                        cmd3.CommandType = CommandType.Text;

        //                        SqlParameter pidPickDetail = new SqlParameter
        //                        {
        //                            ParameterName = "@idPickDetail",
        //                            SqlDbType = SqlDbType.Int,
        //                            Value = idPickDetail
        //                        };
        //                        cmd3.Parameters.Add(idPickDetail);

        //                        dtUnitCost.Load(cmd3.ExecuteReader());

        //                        cmd3.Dispose();
        //                    }
        //                }

        //                cmd2.Dispose();
        //            }
        //        }

        //        cmd.Dispose();
        //    }

        //    connection.Close();



        //    return dtUnitCost;
        //}
    }
}
