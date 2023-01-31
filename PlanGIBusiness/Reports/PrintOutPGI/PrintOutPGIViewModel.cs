using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PlanGIBusiness.Reports
{
    public class PrintOutPGIViewModel
    {
        //header
        public Guid PlanGoodsIssue_Index { get; set; }
        public string PlanGoodsIssue_No { get; set; }
        public string DocumentItem_Remark { get; set; }
        public string PlanGoodsIssue_Date { get; set; }
        public string Warehouse_Name { get; set; }
        public string SoldTo_Name { get; set; }
        public string Sloc_Name { get; set; }
        public string Sloc_Name_To { get; set; }
        public string MovementType_Id { get; set; }

        //detail    
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public decimal? Qty { get; set; }
        public string ProductConversion_Name { get; set; }

        //footer

        public bool isRepresentative { get; set; }
        public string representative_Name { get; set; }

        public bool isEndorser { get; set; }
        public string endorser_Name { get; set; }
        public string endorser_pos_Name { get; set; }

        public bool isTransferor { get; set; }
        public string transferor_Name { get; set; }
        public string transferor_pos_Name { get; set; }

        public bool isTransfer_Approver { get; set; }
        public string transfer_Approver_Name { get; set; }
        public string transfer_Approver_pos_Name { get; set; }

        public bool isRecorder { get; set; }
        public string recorder_Name { get; set; }
        public string recorder_pos_Name { get; set; }

        //log
        public Guid? documentType_Index { get; set; }
        public string documentType_Id { get; set; }

        public string documentType_Name { get; set; }

        public string user { get; set; }
        public string representative_first_Name { get; set; }
        public string representative_last_Name { get; set; }
        public string representative_user_Index { get; set; }
        public string representative_user_Id { get; set; }
        public string representative_user_Name { get; set; }
        public string representative_position_Code { get; set; }

        public string endorser_first_Name { get; set; }
        public string endorser_last_Name { get; set; }
        public string endorser_user_Index { get; set; }
        public string endorser_user_Id { get; set; }
        public string endorser_user_Name { get; set; }
        public string endorser_position_Code { get; set; }

        public string transferor_first_Name { get; set; }
        public string transferor_last_Name { get; set; }
        public string transferor_user_Index { get; set; }
        public string transferor_user_Id { get; set; }
        public string transferor_user_Name { get; set; }
        public string transferor_position_Code { get; set; }

        public string transfer_Approver_first_Name { get; set; }
        public string transfer_Approver_last_Name { get; set; }
        public string transfer_Approver_user_Index { get; set; }
        public string transfer_Approver_user_Id { get; set; }
        public string transfer_Approver_user_Name { get; set; }
        public string transfer_Approver_position_Code { get; set; }

        public string recorder_first_Name { get; set; }
        public string recorder_last_Name { get; set; }
        public string recorder_user_Index { get; set; }
        public string recorder_user_Id { get; set; }
        public string recorder_user_Name { get; set; }
        public string recorder_position_Code { get; set; }
    }
}
