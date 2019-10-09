using POSOINV.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace POSOINV.Functions
{
    public class Import_Shipment_Header
    {
        //public static DataTable RetrieveData(SqlConnection connection, string ImportShipmentNumber)
        //{
        //    StringBuilder sQuery = new StringBuilder();

        //    sQuery.Append(@"SELECT idImpShpHead
        //                    ,ImportShipmentNumber     
        //                    ,Total_Charges
        //                    FROM a_Import_Shipment_Header
        //                    WHERE ImportShipmentNumber LIKE '%' + @ImportShipmentNumber + '%'
        //                    ");

        //    //var lmodel = new List<Import_Shipment_Header_Model>();

        //    DataTable dataTable = new DataTable();

        //    connection.Open();

        //    using (SqlCommand cmd = new SqlCommand())
        //    {
        //        cmd.Connection = connection;
        //        cmd.CommandText = sQuery.ToString();
        //        cmd.CommandType = CommandType.Text;

        //        SqlParameter parm2 = new SqlParameter
        //        {
        //            ParameterName = "@ImportShipmentNumber",
        //            SqlDbType = SqlDbType.NVarChar,
        //            Value = ImportShipmentNumber
        //        };
        //        cmd.Parameters.Add(parm2);

        //        dataTable.Load(cmd.ExecuteReader());
        //        cmd.Dispose();
        //    }

        //    connection.Close();

        //    return dataTable;
        //}

        public static List<Import_Shipment_Header_Model> RetrieveData(SqlConnection connection, int idImpShpHead)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idImpShpHead
      ,ImportShipmentNumber
      ,Brokerage
      ,CEDEC
      ,CustomsStamps
      ,DeliveryCharges
      ,DocumentaryStamps
      ,DocumentationCharges
      ,ForkliftCost
      ,FreightCharges
      ,HandlingFee
      ,ImportDuties
      ,ImportProcessingFee
      ,ImportationInsurance
      ,Miscellaneous
      ,NotarialFee
      ,OtherCharges
      ,ProcessingFee
      ,WarehouseStorageCharges
      ,Xerox
      ,Total_Charges
FROM a_Import_Shipment_Header
WHERE idImpShpHead <> 0
");
            if (idImpShpHead != 0)
            {
                sQuery.Append(@"AND idImpShpHead = @idImpShpHead  ");
            }

            var lmodel = new List<Import_Shipment_Header_Model>();

            DataTable dataTable = new DataTable();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;

                if (idImpShpHead != 0)
                {
                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idImpShpHead",
                        SqlDbType = SqlDbType.Int,
                        Value = idImpShpHead
                    };
                    cmd.Parameters.Add(parm1);
                }

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    Import_Shipment_Header_Model oModel = new Import_Shipment_Header_Model
                    {
                        idImpShpHead = (int)oreader["idImpShpHead"],
                        ImportShipmentNumber = (string)oreader["ImportShipmentNumber"],
                        Brokerage = (decimal)oreader["Brokerage"],
                        CEDEC = (decimal)oreader["CEDEC"],
                        CustomsStamps = (decimal)oreader["CustomsStamps"],
                        DeliveryCharges = (decimal)oreader["DeliveryCharges"],
                        DocumentaryStamps = (decimal)oreader["DocumentaryStamps"],
                        DocumentationCharges = (decimal)oreader["DocumentationCharges"],
                        ForkliftCost = (decimal)oreader["ForkliftCost"],
                        FreightCharges = (decimal)oreader["FreightCharges"],
                        HandlingFee = (decimal)oreader["HandlingFee"],
                        ImportDuties = (decimal)oreader["ImportDuties"],
                        ImportProcessingFee = (decimal)oreader["ImportProcessingFee"],
                        ImportationInsurance = (decimal)oreader["ImportationInsurance"],
                        Miscellaneous = (decimal)oreader["Miscellaneous"],
                        NotarialFee = (decimal)oreader["NotarialFee"],
                        OtherCharges = (decimal)oreader["OtherCharges"],
                        ProcessingFee = (decimal)oreader["ProcessingFee"],
                        WarehouseStorageCharges = (decimal)oreader["WarehouseStorageCharges"],
                        Xerox = (decimal)oreader["Xerox"],
                        Total_Charges = (decimal)oreader["Total_Charges"]
                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static List<Import_Shipment_Header_Model> RetrieveData(SqlConnection connection, string ImportShipmentNumber)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"SELECT idImpShpHead
      ,ImportShipmentNumber
      ,Brokerage
      ,CEDEC
      ,CustomsStamps
      ,DeliveryCharges
      ,DocumentaryStamps
      ,DocumentationCharges
      ,ForkliftCost
      ,FreightCharges
      ,HandlingFee
      ,ImportDuties
      ,ImportProcessingFee
      ,ImportationInsurance
      ,Miscellaneous
      ,NotarialFee
      ,OtherCharges
      ,ProcessingFee
      ,WarehouseStorageCharges
      ,Xerox
      ,Total_Charges
FROM a_Import_Shipment_Header
WHERE idImpShpHead <> 0
");         

            if (ImportShipmentNumber != "")
            {
                sQuery.Append(@"AND ImportShipmentNumber LIKE '%' + @ImportShipmentNumber + '%' ");
            }

            var lmodel = new List<Import_Shipment_Header_Model>();

            DataTable dataTable = new DataTable();

            connection.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sQuery.ToString();
                cmd.CommandType = CommandType.Text;             

                if (ImportShipmentNumber != "")
                {
                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@ImportShipmentNumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = ImportShipmentNumber
                    };
                    cmd.Parameters.Add(parm2);
                }

                var oreader = cmd.ExecuteReader();

                while (oreader.Read())
                {
                    Import_Shipment_Header_Model oModel = new Import_Shipment_Header_Model
                    {
                        idImpShpHead = (int)oreader["idImpShpHead"],
                        ImportShipmentNumber = (string)oreader["ImportShipmentNumber"],
                        Brokerage = (decimal)oreader["Brokerage"],
                        CEDEC = (decimal)oreader["CEDEC"],
                        CustomsStamps = (decimal)oreader["CustomsStamps"],
                        DeliveryCharges = (decimal)oreader["DeliveryCharges"],
                        DocumentaryStamps = (decimal)oreader["DocumentaryStamps"],
                        DocumentationCharges = (decimal)oreader["DocumentationCharges"],
                        ForkliftCost = (decimal)oreader["ForkliftCost"],
                        FreightCharges = (decimal)oreader["FreightCharges"],
                        HandlingFee = (decimal)oreader["HandlingFee"],
                        ImportDuties = (decimal)oreader["ImportDuties"],
                        ImportProcessingFee = (decimal)oreader["ImportProcessingFee"],
                        ImportationInsurance = (decimal)oreader["ImportationInsurance"],
                        Miscellaneous = (decimal)oreader["Miscellaneous"],
                        NotarialFee = (decimal)oreader["NotarialFee"],
                        OtherCharges = (decimal)oreader["OtherCharges"],
                        ProcessingFee = (decimal)oreader["ProcessingFee"],
                        WarehouseStorageCharges = (decimal)oreader["WarehouseStorageCharges"],
                        Xerox = (decimal)oreader["Xerox"],
                        Total_Charges = (decimal)oreader["Total_Charges"]
                    };
                    lmodel.Add(oModel);
                }
                oreader.Close();
                cmd.Dispose();
            }

            connection.Close();

            return lmodel;
        }

        public static int Save(SqlConnection connection, Import_Shipment_Header_Model model)
        {
            int returnValue = 0;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"INSERT INTO a_Import_Shipment_Header
           (ImportShipmentNumber
           ,Brokerage
           ,CEDEC
           ,CustomsStamps
           ,DeliveryCharges
           ,DocumentaryStamps
           ,DocumentationCharges
           ,ForkliftCost
           ,FreightCharges
           ,HandlingFee
           ,ImportDuties
           ,ImportProcessingFee
           ,ImportationInsurance
           ,Miscellaneous
           ,NotarialFee
           ,OtherCharges
           ,ProcessingFee
           ,WarehouseStorageCharges
           ,Xerox
           ,Total_Charges)
     VALUES
           (@ImportShipmentNumber
           ,@Brokerage
           ,@CEDEC
           ,@CustomsStamps
           ,@DeliveryCharges
           ,@DocumentaryStamps
           ,@DocumentationCharges
           ,@ForkliftCost
           ,@FreightCharges
           ,@HandlingFee
           ,@ImportDuties
           ,@ImportProcessingFee
           ,@ImportationInsurance
           ,@Miscellaneous
           ,@NotarialFee
           ,@OtherCharges
           ,@ProcessingFee
           ,@WarehouseStorageCharges
           ,@Xerox
           ,@Total_Charges)

SELECT SCOPE_IDENTITY() as 'ID'");

            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@ImportShipmentNumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.ImportShipmentNumber
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Brokerage",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Brokerage
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@CEDEC",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.CEDEC
                    };
                    cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter
                    {
                        ParameterName = "@CustomsStamps",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.CustomsStamps
                    };
                    cmd.Parameters.Add(parm5);

                    SqlParameter parm6 = new SqlParameter
                    {
                        ParameterName = "@DeliveryCharges",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.DeliveryCharges
                    };
                    cmd.Parameters.Add(parm6);

                    SqlParameter parm7 = new SqlParameter
                    {
                        ParameterName = "@DocumentaryStamps",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.DocumentaryStamps
                    };
                    cmd.Parameters.Add(parm7);

                    SqlParameter parm8 = new SqlParameter
                    {
                        ParameterName = "@DocumentationCharges",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.DocumentationCharges
                    };
                    cmd.Parameters.Add(parm8);

                    SqlParameter parm9 = new SqlParameter
                    {
                        ParameterName = "@ForkliftCost",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.ForkliftCost
                    };
                    cmd.Parameters.Add(parm9);

                    SqlParameter parm10 = new SqlParameter
                    {
                        ParameterName = "@FreightCharges",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.FreightCharges
                    };
                    cmd.Parameters.Add(parm10);

                    SqlParameter parm11 = new SqlParameter
                    {
                        ParameterName = "@HandlingFee",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.HandlingFee
                    };
                    cmd.Parameters.Add(parm11);

                    SqlParameter parm12 = new SqlParameter
                    {
                        ParameterName = "@ImportDuties",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.ImportDuties
                    };
                    cmd.Parameters.Add(parm12);

                    SqlParameter parm13 = new SqlParameter
                    {
                        ParameterName = "@ImportProcessingFee",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.ImportProcessingFee
                    };
                    cmd.Parameters.Add(parm13);

                    SqlParameter parm14 = new SqlParameter
                    {
                        ParameterName = "@ImportationInsurance",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.ImportationInsurance
                    };
                    cmd.Parameters.Add(parm14);

                    SqlParameter parm15 = new SqlParameter
                    {
                        ParameterName = "@Miscellaneous",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Miscellaneous
                    };
                    cmd.Parameters.Add(parm15);

                    SqlParameter parm16 = new SqlParameter
                    {
                        ParameterName = "@NotarialFee",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.NotarialFee
                    };
                    cmd.Parameters.Add(parm16);

                    SqlParameter parm17 = new SqlParameter
                    {
                        ParameterName = "@OtherCharges",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.OtherCharges
                    };
                    cmd.Parameters.Add(parm17);

                    SqlParameter parm18 = new SqlParameter
                    {
                        ParameterName = "@ProcessingFee",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.ProcessingFee
                    };
                    cmd.Parameters.Add(parm18);

                    SqlParameter parm19 = new SqlParameter
                    {
                        ParameterName = "@WarehouseStorageCharges",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.WarehouseStorageCharges
                    };
                    cmd.Parameters.Add(parm19);

                    SqlParameter parm20 = new SqlParameter
                    {
                        ParameterName = "@Xerox",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Xerox
                    };
                    cmd.Parameters.Add(parm20);

                    SqlParameter parm21 = new SqlParameter
                    {
                        ParameterName = "@Total_Charges",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Total_Charges
                    };
                    cmd.Parameters.Add(parm21);

                    var oreader = cmd.ExecuteReader();

                    while (oreader.Read())
                    {
                        returnValue = Convert.ToInt32(oreader["ID"].ToString());
                    }

                    oreader.Close();
                    cmd.Dispose();
                    cmd.Parameters.Clear();
                    SQL_Transact.CommitTransaction(connection, GUID);
                }
                catch
                {
                    cmd.Dispose();
                    cmd.Parameters.Clear();
                    SQL_Transact.RollbackTransaction(connection, GUID);
                }
            }

            return returnValue;
        }

        public static bool Update(SqlConnection connection, Import_Shipment_Header_Model model)
        {
            bool returnValue = true;

            var GUID = SQL_Transact.GenerateGUID();

            SQL_Transact.BeginTransaction(connection, GUID);

            StringBuilder sQuery = new StringBuilder();

            sQuery.Append(@"UPDATE a_Import_Shipment_Header
   SET ImportShipmentNumber = @ImportShipmentNumber
      ,Brokerage = @Brokerage
      ,CEDEC = @CEDEC
      ,CustomsStamps = @CustomsStamps
      ,DeliveryCharges = @DeliveryCharges
      ,DocumentaryStamps = @DocumentaryStamps
      ,DocumentationCharges = @DocumentationCharges
      ,ForkliftCost = @ForkliftCost
      ,FreightCharges = @FreightCharges
      ,HandlingFee = @HandlingFee
      ,ImportDuties = @ImportDuties
      ,ImportProcessingFee = @ImportProcessingFee
      ,ImportationInsurance = @ImportationInsurance
      ,Miscellaneous = @Miscellaneous
      ,NotarialFee = @NotarialFee
      ,OtherCharges = @OtherCharges
      ,ProcessingFee = @ProcessingFee
      ,WarehouseStorageCharges = @WarehouseStorageCharges
      ,Xerox = @Xerox
      ,Total_Charges = @Total_Charges
                             WHERE idImpShpHead = @idImpShpHead ");


            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm1 = new SqlParameter
                    {
                        ParameterName = "@idImpShpHead",
                        SqlDbType = SqlDbType.Int,
                        Value = model.idImpShpHead
                    };
                    cmd.Parameters.Add(parm1);

                    SqlParameter parm2 = new SqlParameter
                    {
                        ParameterName = "@ImportShipmentNumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.ImportShipmentNumber
                    };
                    cmd.Parameters.Add(parm2);

                    SqlParameter parm3 = new SqlParameter
                    {
                        ParameterName = "@Brokerage",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Brokerage
                    };
                    cmd.Parameters.Add(parm3);

                    SqlParameter parm4 = new SqlParameter
                    {
                        ParameterName = "@CEDEC",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.CEDEC
                    };
                    cmd.Parameters.Add(parm4);

                    SqlParameter parm5 = new SqlParameter
                    {
                        ParameterName = "@CustomsStamps",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.CustomsStamps
                    };
                    cmd.Parameters.Add(parm5);

                    SqlParameter parm6 = new SqlParameter
                    {
                        ParameterName = "@DeliveryCharges",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.DeliveryCharges
                    };
                    cmd.Parameters.Add(parm6);

                    SqlParameter parm7 = new SqlParameter
                    {
                        ParameterName = "@DocumentaryStamps",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.DocumentaryStamps
                    };
                    cmd.Parameters.Add(parm7);

                    SqlParameter parm8 = new SqlParameter
                    {
                        ParameterName = "@DocumentationCharges",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.DocumentationCharges
                    };
                    cmd.Parameters.Add(parm8);

                    SqlParameter parm9 = new SqlParameter
                    {
                        ParameterName = "@ForkliftCost",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.ForkliftCost
                    };
                    cmd.Parameters.Add(parm9);

                    SqlParameter parm10 = new SqlParameter
                    {
                        ParameterName = "@FreightCharges",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.FreightCharges
                    };
                    cmd.Parameters.Add(parm10);

                    SqlParameter parm11 = new SqlParameter
                    {
                        ParameterName = "@HandlingFee",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.HandlingFee
                    };
                    cmd.Parameters.Add(parm11);

                    SqlParameter parm12 = new SqlParameter
                    {
                        ParameterName = "@ImportDuties",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.ImportDuties
                    };
                    cmd.Parameters.Add(parm12);

                    SqlParameter parm13 = new SqlParameter
                    {
                        ParameterName = "@ImportProcessingFee",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.ImportProcessingFee
                    };
                    cmd.Parameters.Add(parm13);

                    SqlParameter parm14 = new SqlParameter
                    {
                        ParameterName = "@ImportationInsurance",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.ImportationInsurance
                    };
                    cmd.Parameters.Add(parm14);

                    SqlParameter parm15 = new SqlParameter
                    {
                        ParameterName = "@Miscellaneous",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Miscellaneous
                    };
                    cmd.Parameters.Add(parm15);

                    SqlParameter parm16 = new SqlParameter
                    {
                        ParameterName = "@NotarialFee",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.NotarialFee
                    };
                    cmd.Parameters.Add(parm16);

                    SqlParameter parm17 = new SqlParameter
                    {
                        ParameterName = "@OtherCharges",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.OtherCharges
                    };
                    cmd.Parameters.Add(parm17);

                    SqlParameter parm18 = new SqlParameter
                    {
                        ParameterName = "@ProcessingFee",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.ProcessingFee
                    };
                    cmd.Parameters.Add(parm18);

                    SqlParameter parm19 = new SqlParameter
                    {
                        ParameterName = "@WarehouseStorageCharges",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.WarehouseStorageCharges
                    };
                    cmd.Parameters.Add(parm19);

                    SqlParameter parm20 = new SqlParameter
                    {
                        ParameterName = "@Xerox",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Xerox
                    };
                    cmd.Parameters.Add(parm20);

                    SqlParameter parm21 = new SqlParameter
                    {
                        ParameterName = "@Total_Charges",
                        SqlDbType = SqlDbType.Decimal,
                        Value = model.Total_Charges
                    };
                    cmd.Parameters.Add(parm21);

                    if (cmd.ExecuteNonQuery() >= 1)
                    {
                        returnValue = true;
                        cmd.Dispose();
                        cmd.Parameters.Clear();
                        SQL_Transact.CommitTransaction(connection, GUID);
                    }
                }
                catch
                {
                    cmd.Dispose();
                    cmd.Parameters.Clear();
                    SQL_Transact.RollbackTransaction(connection, GUID);
                }
            }
            return returnValue;
        }

        public static bool Delete(SqlConnection connection, int idImpShpHead)
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.Append("DELETE FROM a_Import_Shipment_Header ");
            sQuery.Append("WHERE idImpShpHead = @idImpShpHead");

            bool boolReturnValue = false;

            try
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sQuery.ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlParameter parm = new SqlParameter
                    {
                        ParameterName = "@idImpShpHead",
                        SqlDbType = SqlDbType.Int,
                        Value = idImpShpHead
                    };
                    cmd.Parameters.Add(parm);

                    if (cmd.ExecuteNonQuery() >= 1)
                        boolReturnValue = true;

                    cmd.Dispose();
                    cmd.Parameters.Clear();
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            return boolReturnValue;
        }
    }
}
