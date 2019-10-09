using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSOINV.Functions
{
    public class Supplier
    {
        public static DataTable RetrieveData(SqlConnection con, string supplier_name)
        {
            DataTable dt = new DataTable();
            StringBuilder strQuery = new StringBuilder();

            strQuery.Append(@"SELECT [abbr_name]
	                                ,[supplier_name]
	                                ,[address_1]
	                                ,[address_2]
	                                ,[address_3]
	                                ,[address_4]
	                          FROM [ERP_Live_Test].[dbo].[a_supplier]
	                          WHERE supplier_name LIKE '%" + supplier_name + @"%'
                              ORDER BY supplier_name");

            con.Open();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(strQuery.ToString(), con))
                {
                    using (DataTable ds = new DataTable())
                    {
                        da.Fill(ds);
                        dt = ds;
                    }
                }
            }
            catch
            {
                SqlConnection.ClearAllPools();
            }
            con.Close();
            SqlConnection.ClearAllPools();
            return dt;
        }
    }
}
