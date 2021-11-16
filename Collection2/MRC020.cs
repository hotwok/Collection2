using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ACT.Common.Data;
using Agresso.Interface.CommonExtension;
using Agresso.Interface.TopGenExtension;
using System.Data;
using System.Windows.Forms;

namespace Collection2
{
    [TopGen("MRC020", "*", "*", "IM Allocation - v1.0")]
    public class MRC020 : IProjectTopGen
    {
        private IForm _form;
        private string newComment;
        private IField fLoadId;
        private bool reYN = false;
        IField fLefttoAlloc;
        IField BankUserName;
        IField BankUserNameR;
        IField payreferenceR;
        IField fAMinvoice;
        

        public void Initialize(IForm form)
        {
            _form = form;
            _form.OnModelLoad += _form_OnModelLoad;
            _form.OnSearchList += _form_OnSearchList;
            _form.OnValidatedField += _form_OnValidatedField;
            _form.OnCalledAction += _form_OnCalledAction;
            _form.OnGetValueList += _form_OnGetValueList;
            _form.OnSaved += _form_OnSaved;
            _form.OnDataSchemaValidate += _form_OnDataSchemaValidate;
            _form.OnDeletedRows += _form_OnDeletedRows;
            _form.OnLoaded += _form_OnLoaded;
            _form.OnScreenRefreshed += _form_OnScreenRefreshed;
            
        }

        private void _form_OnScreenRefreshed(object sender, ScreenRefreshedEventArgs e)
        {
            string fill1 = _form.ViewState.GetRestriction(_form.GetSection("TblIntray"));
            //CurrentContext.Message.Display("OnScreen {0}", fill1);

            if (fill1 == "")
            {
                List<KeyValuePair<string, object>> newFilter = new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("rev_code", "UNA"),
                    new KeyValuePair<string, object>("intray_status", "N")
                };
                _form.ViewState.SetGridFilter(_form.GetSection("TblIntray"), newFilter);
            }

        }

        private void _form_OnLoaded(object sender, LoadEventArgs e)
        {
            //CurrentContext.Message.Display("TEST");
            DataTable dtFrom = _form.Data.Tables["TblIntray"];

            foreach (DataRow dr in dtFrom.Rows)
            {
                string oldValue = (string)dr["Intray_comment"];
                String[] separator = { "BANKUSNM", "ALLOC" };
                //int count = 3;

                //if (oldValue.Length > 12)
                //{
                //    String[] strlist = oldValue.Split(separator, StringSplitOptions.None);

                //    dr["payreference"] = strlist[0];
                //    dr["BankUserName"] = strlist[1];

                //}

                //DataRow row = dt.Rows[0];
                //string oldValue = (string)row["MyColumn"];  // Returns an object. Therefore cast to string.
                //row["MyColumn"] = oldValue?.Split('=')[0];


            }

            ISection TransSec = _form.GetSection("TblAcutrans");


            if (TransSec.GetField("AM_Invoice") != null)
                return;

            fAMinvoice = TransSec.Fields.Create("AM_Invoice", ControlType.TextBox);
            fAMinvoice.IsReadOnly = true;
            fAMinvoice.IsHidden = false;
            fAMinvoice.Title = "test";



        }

        private void _form_OnDeletedRows(object sender, DeleteRowsEventArgs e)
        {
            lefttoallocate();
        }

        private void _form_OnDataSchemaValidate(object sender, DataSchemaValidateEventArgs e)
        {
            //Add to datatable
            //Allocation, middle bit
            ISection recSection1;
            DataTable dt;
            recSection1 = _form.GetSection("SecHeader");
            dt = _form.Data.Tables[recSection1.TableName];

            if (!dt.Columns.Contains("LefttoAlloc"))
                dt.Columns.Add("LefttoAlloc", typeof(double));
            
            //Allocation Top
            ISection SecTop;
            DataTable dt1;
            SecTop = _form.GetSection("SecTop");
            dt1 = _form.Data.Tables[SecTop.TableName];

            if (!dt1.Columns.Contains("BankUserName"))
                dt1.Columns.Add("BankUserName", typeof(string));

            //Receipts tab detail
            ISection TblIntray;
            DataTable dt2;
            TblIntray = _form.GetSection("TblIntray");
            dt2 = _form.Data.Tables[TblIntray.TableName];

            if (!dt2.Columns.Contains("BankUserName"))
                dt2.Columns.Add("BankUserName", typeof(string));

            if (!dt2.Columns.Contains("payreference"))
                dt2.Columns.Add("payreference", typeof(string));


            //Allocation tab - cust inv details
            ISection TransSec;
            DataTable dt3;
            TransSec = _form.GetSection("TblAcutrans");
            dt3 = _form.Data.Tables[TransSec.TableName];

            if (!dt3.Columns.Contains("AM_Invoice"))
                dt3.Columns.Add("AM_Invoice", typeof(string));


        }

        private void _form_OnSaved(object sender, SaveEventArgs e)
        {
           
            //DataRow drSecHead = _form.Data.Tables["SecTop"].Rows[0];

            //IStatement sqlupd = CurrentContext.Database.CreateStatement();

            //sqlupd.Assign(" UPDATE mrcpayimport ");
            //sqlupd.Append(" SET allocation_comment = @alloc ");
            //sqlupd.Append(" WHERE client= @client ");
            //sqlupd.Append(" AND receipt_no = @receipt_no ");
            //sqlupd["client"] = _form.Client;
            //sqlupd["alloc"] = drSecHead["alloc_comment"].ToString();
            //sqlupd["receipt_no"] = drSecHead["alloc_receipt"].ToString();
            //sqlupd["line_no"] = drSecHead["alloc_line_no"].ToString();


            //CurrentContext.Database.Execute(sqlupd);
            //Add the comment back to the Import screen when it jumps back there.
            //What about part allocations - ADD WHERE ALLOCATION STATUS = 'A'

            //string rDetails = string.Format("receipt_no={0}", drSecHead["alloc_receipt"].ToString());
            //rDetails = rDetails + " AND " + string.Format("line_no={0}", drSecHead["alloc_line_no"].ToString());


            //DataRow row = _form.Data.Tables["TblAlloc"].Select(string.Format("receipt_no={0}",drSecHead["alloc_receipt"].ToString())).FirstOrDefault();
            //Choose the row - cater for Multiple allocations by selecting the line no
            //DataRow row = _form.Data.Tables["TblAlloc"].Select(rDetails).FirstOrDefault();
            //row.SetField("allocation_comment", drSecHead["alloc_comment"].ToString());
            
            //Clear the summary table on the Allocation tab so I can do the save myself.
            //Need to copy it first and do the insert statements.
            //_form.Data.Tables["TblSummary"].Rows.Clear();
            //_form.Data.Tables["TblAlloc"].Rows.Clear();

            //_form.Commands.CreateLoadCommand(_form.GetSection("TabAlloc")).Execute();
            //fLoadId.Validate();

        }

        private void _form_OnGetValueList(object sender, ValueListEventArgs e)
        {
            if (e.FieldName == "rev_code")
            {
                IValueDescriptionList list = ValueDescriptionListFactory.Create<string>();
                //A1 = Tax code

                IStatement sql = CurrentContext.Database.CreateStatement();
                sql.Assign("SELECT dim_value, description ");
                sql.Append("  FROM agldimvalue ");
                sql.Append("  WHERE client=@client ");
                sql.Append("  AND attribute_id='IM02' AND status='N'");
                sql.Append(" ORDER BY dim_value ");
                sql["client"] = _form.Client;

                DataTable dt = new DataTable();
                CurrentContext.Database.Read(sql, dt);

                string IncCode = "";
                string IncDesc = "";
                foreach (DataRow row in dt.Rows)
                {
                    IncCode = row[0].ToString();
                    IncDesc = row[1].ToString();
                    list.Add(IncCode, IncDesc);
                }
                e.List = list;
            }
        }

        private void _form_OnCalledAction(object sender, ActionEventArgs e)
        {
            if (e.Action == "Allocation")
            {
                //Get the comment for the row we are allocating from the front screen
                //Update the allocation screen when it is opened 
                //Set ReAllocation = FALSE
                reYN = false;
                newComment = e.Row["allocation_comment"].ToString();
                //Add default comment if blank
                if (string.IsNullOrWhiteSpace(newComment))
                {
                    newComment = ".";
                }
                DataRow drSecTop = _form.Data.Tables["SecTop"].Rows[0];
                //drSecTop.SetField("alloc_comment", e.Row["allocation_comment"].ToString());
                drSecTop.SetField("alloc_comment", newComment);
                drSecTop.SetField("BankUserName", e.Row["bank_user_name"].ToString());
                Clipboard.SetText(e.Row["payreference"].ToString());
                lefttoallocate();
            }

            if (e.Action == "Reallocation")
            {
                //Set ReAllocation = TRUE
                reYN = true;
                //Get the comment for the row we are allocating from the front screen
                //Swap intray_comment with alloc_orig_ref
                //Update the allocation screen when it is opened 
                //

                IStatement sql = CurrentContext.Database.CreateStatement();
                sql.Assign("SELECT DISTINCT payreference2, payreference3, bank_user_name ");
                sql.Append("  FROM mrcpayimport ");
                sql.Append("  WHERE client=@client ");
                sql.Append("  AND receipt_no =@receipt_no ");
                sql["client"] = _form.Client;
                sql["receipt_no"] = e.Row["receipt_no"].ToString();

                DataTable dt = new DataTable();
                CurrentContext.Database.Read(sql, dt);

                newComment = e.Row["intray_comment"].ToString();
                //Add default comment if blank
                if (string.IsNullOrWhiteSpace(newComment))
                {
                    newComment = ".";
                }

                newComment = newComment.Substring(0, Math.Min(80, newComment.Length));

                DataRow drSecTop = _form.Data.Tables["SecTop"].Rows[0];

                if (dt.Rows.Count > 0)
                {
                    
                    //drSecTop.SetField("alloc_comment", e.Row["allocation_comment"].ToString());
                    drSecTop.SetField("alloc_comment", newComment);
                    drSecTop.SetField("alloc_orig_ref2", dt.Rows[0]["payreference2"].ToString());
                    drSecTop.SetField("alloc_orig_ref3", dt.Rows[0]["payreference3"].ToString());
                    drSecTop.SetField("BankUserName", dt.Rows[0]["bank_user_name"].ToString());
                }
                //Clipboard.SetText(e.Row["intray_comment"].ToString());
                string comment = drSecTop["alloc_orig_ref"].ToString();
                if (string.IsNullOrWhiteSpace(comment))
                {
                    comment = "test";
                }
                Clipboard.SetText(comment);
                lefttoallocate();
            }


            if (e.Action == "keepIncome" || e.Action == "keepDebtor" || e.Action == "keepExternal")
            {
                //tblsummary - update allocation_comment - col 26
                //CurrentContext.Message.Display("keep");
                lefttoallocate();
                //int rCount = _form.Data.Tables["TblSummary"].Rows.Count - 1;
            
            }

            if (e.Action == "UpdateAcutransGrid")
            {
                //CurrentContext.Message.Display(e.Action.ToString());
                e.Row.SetField<string>("AM_Invoice", "TEST");
                e.Row.SetField<string>("dim_7_desc", "TEST1");
            }


        }


        private void _form_OnValidatedField(object sender, ValidateFieldEventArgs e)
        {
            if (e.FieldName == "alloc_comment")
            {
                //Copy the comment so when we go back to the import tab we can apply it there
                //Different Reallocate v Allocate
                
                newComment = e.Row["alloc_comment"].ToString();
                    

                if (!string.IsNullOrWhiteSpace(newComment))
                { 
                IStatement sqlupd = CurrentContext.Database.CreateStatement();

                    string uTable = "";
                    string uComment = "";
                    if (reYN)
                    {
                        uTable = "mrcintray";
                        uComment = "intray";
                    }
                    else
                    {
                        uTable = "mrcpayimport";
                        uComment = "allocation";
                    }

                sqlupd.Assign(" UPDATE " + uTable );
                sqlupd.Append(" SET " + uComment + "_comment = @alloc ");
                sqlupd.Append(" WHERE client= @client ");
                sqlupd.Append(" AND receipt_no = @receipt_no ");
                sqlupd.Append(" and line_no = @line_no ");
                sqlupd["client"] = _form.Client;
                sqlupd["alloc"] = e.Row["alloc_comment"].ToString();
                sqlupd["receipt_no"] = e.Row["alloc_receipt"].ToString();
                sqlupd["line_no"] = e.Row["alloc_line_no"].ToString();

                CurrentContext.Database.Execute(sqlupd);

                    if (!reYN)
                    {
                        ISection ImpTabSec = _form.GetSection("TabAlloc");
                        ImpTabSec.IsReadOnly = false;
                    }
                    else
                    {
                        ISection TabIntray = _form.GetSection("TabIntray");
                        TabIntray.IsReadOnly = false;
                    }

                }

            }

            if (e.FieldName == "rev_code")
            {
                //We don't want errors so clear them
                string vRevCode = e.Row["rev_code"].ToString();
                //Reset fields used by another rev code
                //Standard code leaves them in
                e.Row.SetField("address", string.Empty);
                e.Row.SetField("customer", string.Empty);

                if (string.IsNullOrWhiteSpace(vRevCode))
                {

                    if (!reYN)
                    {
                        e.Row.CancelEdit();
                        e.Row.ClearErrors();
                        e.Cancel = true;


                        ISection ImpTabSec1 = _form.GetSection("TblAlloc");
                        ImpTabSec1.ActivateTab();
                        DataRow drActive = ImpTabSec1.GetSelectedDataRow();
                        drActive.SetField("allocation_comment", newComment);
                    }
                    else
                    {
                        e.Row.CancelEdit();
                        e.Row.ClearErrors();
                        e.Cancel = true;

                        ISection RecTabInt = _form.GetSection("TblIntray");
                        RecTabInt.ActivateTab();
                        DataRow drActive = RecTabInt.GetSelectedDataRow();
                        drActive.SetField("intray_comment", newComment);


                    }
                    newComment = "";
                }


            }
            if (e.FieldName == "select_row")
            {
                //CurrentContext.Message.Display("test");
                e.Row.SetField("_mark_row_column", e.Row.Field<bool>(e.FieldName));

            }

            if (e.FieldName == "xaccount")
            {
                if (_form.Data.Tables["TblExternal"].Rows.Count > 0)
                {
                    DataRow tbExternal = _form.Data.Tables["TblExternal"].Rows[0];

                    tbExternal.SetField("_mark_row_column", true);
                    tbExternal.SetField("pay_amount", e.Row.Field<double>("LefttoAlloc"));
                }                
            }

            if (e.FieldName == "customer")
            {
                CurrentContext.Message.Display("here");
                //Get customer - call sql function

                //get TblAcutrans and update
                DataTable TblAcutrans = _form.Data.Tables["TblAcutrans"];
                string apar_id = e.Row.Field<string>("customer");
                DataTable custTrans =  SQLFunc.CustTrans(apar_id);


                foreach (DataRow row in TblAcutrans.Rows)
                {
                    DataRow rowsToUpdate = custTrans.AsEnumerable().FirstOrDefault(r => r.Field<Int64>("voucher_no") == row.Field<Int64>("voucher_no"));
                    row.SetField("AM_invoice", rowsToUpdate.Field<string>("PCBInvno"));
                }



            }

        }



        private void _form_OnSearchList(object sender, ValueListEventArgs e)
        {
            if (e.FieldName == "rev_code")
            {
                IValueDescriptionList list = ValueDescriptionListFactory.Create<string>();
                //A1 = Tax code

                IStatement sql = CurrentContext.Database.CreateStatement();
                sql.Assign("SELECT dim_value, description ");
                sql.Append("  FROM agldimvalue ");
                sql.Append("  WHERE client=@client ");
                sql.Append("  AND attribute_id='IM02' AND status='N'");
                sql.Append(" ORDER BY dim_value ");
                sql["client"] = _form.Client;

                DataTable dt = new DataTable();
                CurrentContext.Database.Read(sql, dt);

                string IncCode = "";
                string IncDesc = "";
                foreach (DataRow row in dt.Rows)
                {
                    IncCode = row[0].ToString();
                    IncDesc = row[1].ToString();
                    list.Add(IncCode, IncDesc);
                }
                e.List = list;
            }

        }

        

        private void _form_OnModelLoad(object sender, ModelLoadEventArgs e)
        {
            //New A&M Field
            ISection TransSec = _form.GetSection("TblAcutrans");
            

            if (TransSec.GetField("AM_Invoice") != null)
                return;

            fAMinvoice = TransSec.Fields.Create("AM_Invoice", ControlType.TextBox);
            fAMinvoice.IsReadOnly = true;
            fAMinvoice.IsHidden = false;
            fAMinvoice.Title = "CIPInvoice";



            //Type Ahead Rev code
            ISection recSection = _form.GetSection("SecHeader");
            IField rev_code = recSection.GetField("rev_code");
            rev_code.ControlType = ControlType.DropdownList;
            rev_code.SearchOnDescription = true;
            rev_code.HasFieldHelp = true;
            rev_code.ShowCode = true;
            rev_code.IsMandatory = false;
            rev_code.HasValidation = true;
            rev_code.HasValidationOnLeave = true;

            if (recSection.GetField("LefttoAlloc") != null)
                return;
            fLefttoAlloc = recSection.Fields.Create("LefttoAlloc", ControlType.TextBox);
            fLefttoAlloc.IsReadOnly = true;
            fLefttoAlloc.DisplayFormat = "8.2";
            fLefttoAlloc.Title = "Left to Allocate";
            fLefttoAlloc.DisplayType = 3;       

            IField address = recSection.GetField("address");
            address.IsReadOnly = true;



            //Save Comment/hide seq/hide area
            ISection SecTop = _form.GetSection("SecTop");
            IField alloc_area = SecTop.GetField("alloc_area");
            alloc_area.IsHidden = true;
            IField alloc_line_seq = SecTop.GetField("alloc_line_seq");
            alloc_line_seq.IsHidden = true;
            IField fComment = SecTop.GetField("alloc_comment");
            fComment.HasValidationOnLeave = true;
            fComment.HasValidation = true;
            IField alloc_orig_ref = SecTop.GetField("alloc_orig_ref");
            alloc_orig_ref.SequenceNo = 13;
            alloc_orig_ref.UsesFullWidth = true;



            if (SecTop.GetField("BankUserName") != null)
                return;
            BankUserName = SecTop.Fields.Create("BankUserName", ControlType.TextBox);
            BankUserName.IsReadOnly = true;
            BankUserName.Title = "Bank User Name";
            BankUserName.SequenceNo = 11;

            if (SecTop.GetField("linebreak3") != null)
                return;
            IField linebreak3 = SecTop.Fields.Create("linebreak3", ControlType.LineBreak);
            linebreak3.IsReadOnly = true;
            BankUserName.SequenceNo = 12;




            //Allocation 
            ISection TblAlloc = _form.GetSection("TblAlloc");
            fLoadId = TblAlloc.GetField("load_id");
            fLoadId.HasValidation = true;

            ISection HdrIntray = _form.GetSection("HdrIntray");
            IField fIntrayFilter = HdrIntray.GetField("IntrayFilter");

            //Receipts - add fields
            ISection TblIntray = _form.GetSection("TblIntray");
            if (TblIntray.GetField("BankUserName") != null)
                return;
            BankUserNameR = TblIntray.Fields.Create("BankUserName", ControlType.TextBox);
            BankUserNameR.IsReadOnly = true;
            BankUserNameR.Title = "Bank User Name";

            
            if (TblIntray.GetField("payreference") != null)
                return;
            payreferenceR = TblIntray.Fields.Create("payreference", ControlType.TextBox);
            payreferenceR.IsReadOnly = true;
            payreferenceR.Title = "Payreference";



        }

        private void lefttoallocate()
        {
            DataRow sDR = _form.Data.Tables["SecTop"].Rows[0];
            //sDR.SetField("Allocation_comment", newComment);
            double lefttoalloc = 0;
            lefttoalloc = Convert.ToDouble(sDR["alloc_amount"].ToString()) - Convert.ToDouble(sDR["alloc_allocated"].ToString());
            //e.Row.SetField("LefttoAlloc",lefttoalloc);
        
            _form.Data.Tables["SecHeader"].Rows[0].SetField("LefttoAlloc", lefttoalloc);


        }
    }
}
