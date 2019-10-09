using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using POSOINV.Functions;
using POSOINV.Models;
using System.Data;
using System.Globalization;
using Microsoft.Reporting.WebForms;
using System.Web.Services;
using System.Net;
using System.IO;
using System.Web;
using System.Configuration;

namespace SHIELD.ERP
{
    public partial class DownloadSO : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DownloadFile();
            }
        }

        protected void DownloadFile()
        {
            byte[] bytes;
            string fileName;
            string contentType;
            var idSO = Session["SOHeaderID"];

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ISConString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT FileName, content_type, data FROM a_SO_Attachment WHERE idSOHeader=@idSOHeader";
                    cmd.Parameters.AddWithValue("@idSOHeader", idSO);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["data"];
                        contentType = sdr["content_type"].ToString();
                        fileName = sdr["FileName"].ToString();
                    }
                    con.Close();
                }
            }

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + ".pdf");
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }

    }
}