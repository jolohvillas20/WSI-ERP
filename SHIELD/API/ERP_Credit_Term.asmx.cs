using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Configuration;
using POSOINV.Models;

namespace SOPOINV.API
{
    /// <summary>
    /// Summary description for ERP_Credit_Term
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ERP_Credit_Term : System.Web.Services.WebService
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString);

        //[WebMethod()]
        //public DataTable getCreditTerms()
        //{
        //    DataTable dt = new DataTable("dtCreditTerm");

        //    StringBuilder strQuery = new StringBuilder();

        //    strQuery.Append(@"SELECT [credit_term]
        //                    ,[term_desc]
        //                    FROM a_Credit_Term");

        //    connection.Open();
        //    try
        //    {
        //        using (SqlDataAdapter da = new SqlDataAdapter(strQuery.ToString(), connection))
        //        {
        //            using (DataTable ds = new DataTable())
        //            {
        //                da.Fill(ds);
        //                dt = ds;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        SqlConnection.ClearAllPools();
        //    }
        //    connection.Close();
        //    return dt;
        //}

        [WebMethod()]
        public List<Credit_Term_Model> getCreditTerms()
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT [credit_term]
                            ,[term_desc]
                            FROM a_Credit_Term
                         ");

            var lmodel = new List<Credit_Term_Model>();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    Credit_Term_Model oModel = new Credit_Term_Model
                    {
                        credit_term = (string)oreader["credit_term"],
                        term_desc = (string)oreader["term_desc"]
                    };
                    lmodel.Add(oModel);
                }

                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }
    }
}
