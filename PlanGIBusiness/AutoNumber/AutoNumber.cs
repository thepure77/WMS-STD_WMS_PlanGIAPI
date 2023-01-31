
using System.Text;
using Business.Library;
using Comone.Utils;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using MasterDataBusiness.ViewModels;

namespace planGIBusiness.AutoNumber
{
    public class AutoNumberService
    {

        public String genAutoDocmentNumber(List<GenDocumentTypeViewModel> DocumentType , DateTime  Document_Date)
        {
            String Document_No = "";
            try
            {

                var DocumentType_Index = DocumentType.FirstOrDefault().documentType_Index;
                var IsResetByYear = DocumentType.FirstOrDefault().isResetByYear;
                var IsResetByMonth = DocumentType.FirstOrDefault().isResetByMonth;
                var IsResetByDay = DocumentType.FirstOrDefault().isResetByDay;

                var Format_Text = DocumentType.FirstOrDefault().format_Text;
                var Format_Date = DocumentType.FirstOrDefault().format_Date;
                var Format_Running = DocumentType.FirstOrDefault().format_Running;
                var Format_Document = DocumentType.FirstOrDefault().format_Document;

                var SetFormatDate = Document_Date.ToString("dd/MM/yyyy");
                var splitDate = SetFormatDate.Split('/');
                var Doc_Year = splitDate[2].ToString();
                var Doc_Month = splitDate[1].ToString();
                var Doc_Day = splitDate[0].ToString();


                
                int iRunningNumber = 0;


                //GetRunning
                if (IsResetByYear == 1)
                {

                    using (var context = new PlanGIDbContext())
                    {

                        var ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),DocumentTypeNumber_Index)");
                        var ColumnName2 = new SqlParameter("@ColumnName2", "Convert(Nvarchar(50),DocumentType_Index)");
                        var ColumnName3 = new SqlParameter("@ColumnName3", "''");
                        var ColumnName4 = new SqlParameter("@ColumnName4", "''");
                        var ColumnName5 = new SqlParameter("@ColumnName5", "''");
                        var TableName = new SqlParameter("@TableName", "ms_DocumentTypeNumber");
                        String sqlWhere = " where DocumentType_Index  = '" + DocumentType_Index.ToString() + "'" +
                                         " and DocumentTypeNumber_Year  = '" + Doc_Year.ToString() + "'";
                                         //" and DocumentTypeNumber_Month  = '" + DocumentType_Index.ToString() + "'" +
                                         //" and DocumentTypeNumber_Day  = '" + DocumentType_Index.ToString() + "'" ;
                        var Where = new SqlParameter("@Where", sqlWhere);
                        var DataDocumentTypeNumber = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).FirstOrDefault();

                        if (DataDocumentTypeNumber != null)
                        {
                            context.Entry(DataDocumentTypeNumber).State = EntityState.Detached;
                        }

                        String sqlCmd = "";

                        Guid DocumentTypeNumber_Index = Guid.NewGuid();

                        if (DataDocumentTypeNumber != null)
                        {

                            DocumentTypeNumber_Index = new Guid(DataDocumentTypeNumber.dataincolumn1); 

                            sqlCmd = "  UPDATE ms_DocumentTypeNumber set " +
                                     "       DocumentTypeNumber_Running = DocumentTypeNumber_Running + 1 " +
                                     "       , Update_By = 'System' " +
                                     "       , Update_Date = getdate() " +
                                     "  where DocumentTypeNumber_Index = '" + DocumentTypeNumber_Index.ToString() + "'" +
                                     "    and DocumentType_Index  = '" + DocumentType_Index.ToString() + "'" +
                                     "    and DocumentTypeNumber_Year  = '" + Doc_Year.ToString() + "'";
                        }
                        else
                        {
                            sqlCmd = "  INSERT INTO[dbo].[ms_DocumentTypeNumber] (      " +
                                   "                      [DocumentTypeNumber_Index]    " +
                                   "                     ,[DocumentType_Index]          " +
                                   "                     ,[DocumentTypeNumber_Year]     " +
                                   "                     ,[DocumentTypeNumber_Month]    " +
                                   "                     ,[DocumentTypeNumber_Day]      " +
                                   "                     ,[DocumentTypeNumber_Running]  " +
                                   "                     ,[IsActive]                    " +
                                   "                     ,[IsDelete]                    " +
                                   "                     ,[IsSystem]                    " +
                                   "                     ,[Status_Id]                   " +
                                   "                     ,[Create_By]                   " +
                                   "                     ,[Create_Date]                 " +
                                   "                     ) VALUES(                      " +
                                   "                       '" + DocumentTypeNumber_Index.ToString() + "'" +
                                   "                     , '" + DocumentType_Index.ToString() + "'" +
                                   "                     , '" + Doc_Year.ToString() + "'"   +
                                   "                     ,''                              " +
                                   "                     ,''                       " +
                                   "                     ,1                        " +
                                   "                     ,1                        " +
                                   "                     ,0                        " +
                                   "                     ,1                        " +
                                   "                     ,0                        " +
                                   "                     ,'System'                 " +
                                   "                     , getdate()               " +
                                   "                    )                         ";
                        }


                        // Start Trasaction get Running
                        var transaction = context.Database.BeginTransaction(IsolationLevel.Serializable);
                        try
                        {
                            context.Database.ExecuteSqlCommand(sqlCmd);


                            ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),DocumentTypeNumber_Index)");
                            ColumnName2 = new SqlParameter("@ColumnName2", "Convert(Nvarchar(50),DocumentType_Index)");
                            ColumnName3 = new SqlParameter("@ColumnName3", "Convert(Nvarchar(50),DocumentTypeNumber_Running)");
                            ColumnName4 = new SqlParameter("@ColumnName4", "''");
                            ColumnName5 = new SqlParameter("@ColumnName5", "''");
                            TableName = new SqlParameter("@TableName", "ms_DocumentTypeNumber");
                            sqlWhere = " where DocumentTypeNumber_Index = '" + DocumentTypeNumber_Index.ToString() + "'";

                            Where = new SqlParameter("@Where", sqlWhere);
                            var DataDocumentTypeNumberNew = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).FirstOrDefault();

                            transaction.Commit();
                            // End Trasaction get Running


                            iRunningNumber = int.Parse(DataDocumentTypeNumberNew.dataincolumn3);

                        }
                        catch (Exception exTrans)
                        {
                            transaction.Rollback();
                            throw exTrans;
                        }




                    }

                }
                else if (IsResetByMonth == 1 )
                {
                    using (var context = new PlanGIDbContext())
                    {

                        var ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),DocumentTypeNumber_Index)");
                        var ColumnName2 = new SqlParameter("@ColumnName2", "Convert(Nvarchar(50),DocumentType_Index)");
                        var ColumnName3 = new SqlParameter("@ColumnName3", "''");
                        var ColumnName4 = new SqlParameter("@ColumnName4", "''");
                        var ColumnName5 = new SqlParameter("@ColumnName5", "''");
                        var TableName = new SqlParameter("@TableName", "ms_DocumentTypeNumber");
                        String sqlWhere = " where DocumentType_Index  = '" + DocumentType_Index.ToString() + "'" +
                                         " and DocumentTypeNumber_Year  = '" + Doc_Year.ToString() + "'" +
                                        " and DocumentTypeNumber_Month  = '" + Doc_Month.ToString() + "'";
                        //" and DocumentTypeNumber_Day  = '" + DocumentType_Index.ToString() + "'" ;
                        var Where = new SqlParameter("@Where", sqlWhere);
                        var DataDocumentTypeNumber = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).FirstOrDefault();

                        if (DataDocumentTypeNumber != null)
                        {
                            context.Entry(DataDocumentTypeNumber).State = EntityState.Detached;
                        }

                        String sqlCmd = "";

                        Guid DocumentTypeNumber_Index = Guid.NewGuid();

                        if (DataDocumentTypeNumber != null)
                        {

                            DocumentTypeNumber_Index = new Guid(DataDocumentTypeNumber.dataincolumn1);

                            sqlCmd = "  UPDATE ms_DocumentTypeNumber set " +
                                     "       DocumentTypeNumber_Running = DocumentTypeNumber_Running + 1 " +
                                     "       , Update_By = 'System' " +
                                     "       , Update_Date = getdate() " +
                                     "  where DocumentTypeNumber_Index = '" + DocumentTypeNumber_Index.ToString() + "'" +
                                     "    and DocumentType_Index  = '" + DocumentType_Index.ToString() + "'" +
                                     "    and DocumentTypeNumber_Year  = '" + Doc_Year.ToString() + "'" +
                                     "    and DocumentTypeNumber_Month  = '" + Doc_Month.ToString() + "'";
                        }
                        else
                        {
                            sqlCmd = "  INSERT INTO[dbo].[ms_DocumentTypeNumber] (      " +
                                   "                      [DocumentTypeNumber_Index]    " +
                                   "                     ,[DocumentType_Index]          " +
                                   "                     ,[DocumentTypeNumber_Year]     " +
                                   "                     ,[DocumentTypeNumber_Month]    " +
                                   "                     ,[DocumentTypeNumber_Day]      " +
                                   "                     ,[DocumentTypeNumber_Running]  " +
                                   "                     ,[IsActive]                    " +
                                   "                     ,[IsDelete]                    " +
                                   "                     ,[IsSystem]                    " +
                                   "                     ,[Status_Id]                   " +
                                   "                     ,[Create_By]                   " +
                                   "                     ,[Create_Date]                 " +
                                   "                     ) VALUES(                      " +
                                   "                       '" + DocumentTypeNumber_Index.ToString() + "'" +
                                   "                     , '" + DocumentType_Index.ToString() + "'" +
                                   "                     , '" + Doc_Year.ToString() + "'" +
                                   "                     , '" + Doc_Month.ToString() + "'" +
                                   "                     ,''                       " +
                                   "                     ,1                        " +
                                   "                     ,1                        " +
                                   "                     ,0                        " +
                                   "                     ,1                        " +
                                   "                     ,0                        " +
                                   "                     ,'System'                 " +
                                   "                     , getdate()               " +
                                   "                    )                         ";
                        }


                        // Start Trasaction get Running
                        var transaction = context.Database.BeginTransaction(IsolationLevel.Serializable);
                        try
                        {
                            context.Database.ExecuteSqlCommand(sqlCmd);


                            ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),DocumentTypeNumber_Index)");
                            ColumnName2 = new SqlParameter("@ColumnName2", "Convert(Nvarchar(50),DocumentType_Index)");
                            ColumnName3 = new SqlParameter("@ColumnName3", "Convert(Nvarchar(50),DocumentTypeNumber_Running)");
                            ColumnName4 = new SqlParameter("@ColumnName4", "''");
                            ColumnName5 = new SqlParameter("@ColumnName5", "''");
                            TableName = new SqlParameter("@TableName", "ms_DocumentTypeNumber");
                            sqlWhere = " where DocumentTypeNumber_Index = '" + DocumentTypeNumber_Index.ToString() + "'";

                            Where = new SqlParameter("@Where", sqlWhere);
                            var DataDocumentTypeNumberNew = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).FirstOrDefault();

                            transaction.Commit();
                            // End Trasaction get Running


                            iRunningNumber = int.Parse(DataDocumentTypeNumberNew.dataincolumn3);

                        }
                        catch (Exception exTrans)
                        {
                            transaction.Rollback();
                            throw exTrans;
                        }




                    }

                }
                else if (IsResetByDay == 1)
                {
                    using (var context = new PlanGIDbContext())
                    {

                        var ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),DocumentTypeNumber_Index)");
                        var ColumnName2 = new SqlParameter("@ColumnName2", "Convert(Nvarchar(50),DocumentType_Index)");
                        var ColumnName3 = new SqlParameter("@ColumnName3", "''");
                        var ColumnName4 = new SqlParameter("@ColumnName4", "''");
                        var ColumnName5 = new SqlParameter("@ColumnName5", "''");
                        var TableName = new SqlParameter("@TableName", "ms_DocumentTypeNumber");
                        String sqlWhere = " where DocumentType_Index  = '" + DocumentType_Index.ToString() + "'" +
                                         " and DocumentTypeNumber_Year  = '" + Doc_Year.ToString() + "'" +
                                        " and DocumentTypeNumber_Month  = '" + Doc_Month.ToString() + "'" + 
                                        " and DocumentTypeNumber_Day  = '" + Doc_Day.ToString() + "'" ;

                        var Where = new SqlParameter("@Where", sqlWhere);
                        var DataDocumentTypeNumber = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).FirstOrDefault();

                        context.Entry(DataDocumentTypeNumber).State = EntityState.Detached;

                        String sqlCmd = "";

                        Guid DocumentTypeNumber_Index = Guid.NewGuid();

                        if (DataDocumentTypeNumber != null)
                        {

                            DocumentTypeNumber_Index = new Guid(DataDocumentTypeNumber.dataincolumn1);

                            sqlCmd = "  UPDATE ms_DocumentTypeNumber set " +
                                     "       DocumentTypeNumber_Running = DocumentTypeNumber_Running + 1 " +
                                     "       , Update_By = 'System' " +
                                     "       , Update_Date = getdate() " +
                                     "  where DocumentTypeNumber_Index = '" + DocumentTypeNumber_Index.ToString() + "'" +
                                     "    and DocumentType_Index  = '" + DocumentType_Index.ToString() + "'" +
                                     "    and DocumentTypeNumber_Year  = '" + Doc_Year.ToString() + "'" +
                                     "    and DocumentTypeNumber_Month  = '" + Doc_Month.ToString() + "'" + 
                                     "    and DocumentTypeNumber_Day  = '" + Doc_Day.ToString() + "'";
                        }
                        else
                        {
                            sqlCmd = "  INSERT INTO[dbo].[ms_DocumentTypeNumber] (      " +
                                   "                      [DocumentTypeNumber_Index]    " +
                                   "                     ,[DocumentType_Index]          " +
                                   "                     ,[DocumentTypeNumber_Year]     " +
                                   "                     ,[DocumentTypeNumber_Month]    " +
                                   "                     ,[DocumentTypeNumber_Day]      " +
                                   "                     ,[DocumentTypeNumber_Running]  " +
                                   "                     ,[IsActive]                    " +
                                   "                     ,[IsDelete]                    " +
                                   "                     ,[IsSystem]                    " +
                                   "                     ,[Status_Id]                   " +
                                   "                     ,[Create_By]                   " +
                                   "                     ,[Create_Date]                 " +
                                   "                     ) VALUES(                      " +
                                   "                       '" + DocumentTypeNumber_Index.ToString() + "'" +
                                   "                     , '" + DocumentType_Index.ToString() + "'" +
                                   "                     , '" + Doc_Year.ToString() + "'" +
                                   "                     , '" + Doc_Month.ToString() + "'" +
                                   "                     , '" + Doc_Day.ToString() + "'" +
                                   "                     ,1                        " +
                                   "                     ,1                        " +
                                   "                     ,0                        " +
                                   "                     ,1                        " +
                                   "                     ,0                        " +
                                   "                     ,'System'                 " +
                                   "                     , getdate()               " +
                                   "                    )                         ";
                        }


                        // Start Trasaction get Running
                        var transaction = context.Database.BeginTransaction(IsolationLevel.Serializable);
                        try
                        {
                            context.Database.ExecuteSqlCommand(sqlCmd);


                            ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),DocumentTypeNumber_Index)");
                            ColumnName2 = new SqlParameter("@ColumnName2", "Convert(Nvarchar(50),DocumentType_Index)");
                            ColumnName3 = new SqlParameter("@ColumnName3", "Convert(Nvarchar(50),DocumentTypeNumber_Running)");
                            ColumnName4 = new SqlParameter("@ColumnName4", "''");
                            ColumnName5 = new SqlParameter("@ColumnName5", "''");
                            TableName = new SqlParameter("@TableName", "ms_DocumentTypeNumber");
                            sqlWhere = " where DocumentTypeNumber_Index = '" + DocumentTypeNumber_Index.ToString() + "'";

                            Where = new SqlParameter("@Where", sqlWhere);
                            var DataDocumentTypeNumberNew = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).FirstOrDefault();

                            transaction.Commit();
                            // End Trasaction get Running


                            iRunningNumber = int.Parse(DataDocumentTypeNumberNew.dataincolumn3);

                        }
                        catch (Exception exTrans)
                        {
                            transaction.Rollback();
                            throw exTrans;
                        }




                    }
                }

                // Set Format_Running

                int iLen = Format_Running.Length;
                string strRunningNumber = "";
                switch (iLen)
                {
                    case 7:
                        strRunningNumber = iRunningNumber.ToString("0000000");
                        break;
                    case 6:
                        strRunningNumber = iRunningNumber.ToString("000000");
                        break;
                    case 5:
                        strRunningNumber = iRunningNumber.ToString("00000");
                        break;
                    case 4:
                        strRunningNumber = iRunningNumber.ToString("0000");
                        break;
                    case 3:
                        strRunningNumber = iRunningNumber.ToString("000");
                        break;
                    case 2:
                        strRunningNumber = iRunningNumber.ToString("00");
                        break;
                    case 1:
                        strRunningNumber = iRunningNumber.ToString();
                        break;
                    default:
                        strRunningNumber = iRunningNumber.ToString("00000000");
                        break;
                }

                // Set Format_Date
                string strRunningDate = "";
                switch (Format_Date)
                {
                    case "yyyyMMdd":
                        strRunningDate = Doc_Year + Doc_Month + Doc_Day;
                        break;
                    case "yyyyMM":
                        strRunningDate = Doc_Year + Doc_Month ;
                        break;
                    case "yyyy":
                        strRunningDate = Doc_Year;
                        break;
                    case "yyMMdd":
                        strRunningDate = Doc_Year.Substring(2, 2) + Doc_Month + Doc_Day;
                        break;
                    case "yyMM":
                        strRunningDate = Doc_Year.Substring(2, 2) + Doc_Month ;
                        break;
                    case "yy":
                        strRunningDate = Doc_Year.Substring(2, 2);
                        break;
                    default:
                        strRunningDate = Doc_Year;
                        break;
                }
                // Set Format_Document

                Document_No = Format_Document;
                Document_No = Document_No.Replace("[Format_Text]", Format_Text);
                Document_No = Document_No.Replace("[Format_Date]", strRunningDate);
                Document_No = Document_No.Replace("[Format_Running]", strRunningNumber);
                Document_No = Document_No.Replace(" ", "");

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Document_No;
        }





        }
}
