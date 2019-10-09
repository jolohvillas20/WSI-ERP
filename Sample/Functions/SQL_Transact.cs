using POSOINV.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSOINV.Functions
{
    public class SQL_Transact
    {
        public static string GenerateGUID()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var stringChars = new char[15];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var GUID = new string(stringChars);

            return GUID;
        }

        public static bool BeginTransaction(SqlConnection connection, string GUID)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                sQuery.Append("BEGIN TRAN " + GUID);
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    if (cmd.ExecuteNonQuery() >= 1)
                        returnValue = true;

                    cmd.Dispose();
                    cmd.Parameters.Clear();
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            return returnValue;
        }

        public static bool CommitTransaction(SqlConnection connection, string GUID)
        {
            bool returnValue = true;
            StringBuilder sQuery = new StringBuilder();
            sQuery.Append(@"COMMIT TRAN " + GUID);

            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {

                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    if (cmd.ExecuteNonQuery() >= 1)
                        returnValue = true;

                    cmd.Dispose();
                    cmd.Parameters.Clear();
                    
                    connection.Close();
                    SqlConnection.ClearAllPools();
                }
                catch (Exception ex)
                {
                    cmd.Dispose();
                    throw new ArgumentException(ex.Message);
                }
            }        
            return returnValue;
        }

    public static bool RollbackTransaction(SqlConnection connection, string GUID)
    {
        bool returnValue = true;
        StringBuilder sQuery = new StringBuilder();

        try
        {
            sQuery.Append(@"ROLLBACK TRAN " + GUID);

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (cmd.ExecuteNonQuery() >= 1)
                    returnValue = true;

                cmd.Dispose();
                cmd.Parameters.Clear();
            }

            connection.Close();
            SqlConnection.ClearAllPools();
        }
        catch (Exception ex)
        {
            throw new ArgumentException(ex.Message);
        }

        return returnValue;
    }
}
}
