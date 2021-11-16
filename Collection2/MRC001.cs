using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ACT.Common.Data;
using Agresso.Interface.CommonExtension;
using Agresso.Interface.TopGenExtension;
using System.Data;

namespace Collection2
{
    [TopGen("MRC001", "*", "*", "IM Receipt Entry - v1.0")]
    public class MRC001 : IProjectTopGen
    {
        private IForm _form;

        public void Initialize(IForm form)
        {
            _form = form;
            _form.OnModelLoad += _form_OnModelLoad;
            _form.OnCalledAction += _form_OnCalledAction;
            _form.OnSearchList += _form_OnSearchList;
            _form.OnGetValueList += _form_OnGetValueList;
            _form.OnValidatedField += _form_OnValidatedField;
            
        }

        private void _form_OnValidatedField(object sender, ValidateFieldEventArgs e)
        {
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
                }
            }

            if (e.FieldName == "rev_code")
            {
                //We don't want errors so clear them
                //string vRevCode = e.Row["rev_code"].ToString();
                //Reset fields used by another rev code
                //Standard code leaves them in
                e.Row.SetField("address", string.Empty);
                e.Row.SetField("customer", string.Empty);
            }


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

        private void _form_OnSearchList(object sender, ValueListEventArgs e)
        {
            //if (e.FieldName == "rev_code")
            //{
            //    IValueDescriptionList list = ValueDescriptionListFactory.Create<string>();
            //    //A1 = Tax code

            //    IStatement sql = CurrentContext.Database.CreateStatement();
            //    sql.Assign("SELECT dim_value, description ");
            //    sql.Append("  FROM agldimvalue ");
            //    sql.Append("  WHERE client='EN' AND attribute_id='IM02' AND status='N'");
            //    sql.Append(" ORDER BY dim_value ");

            //    DataTable dt = new DataTable();
            //    CurrentContext.Database.Read(sql, dt);

            //    string IncCode = "";
            //    string IncDesc = "";
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        IncCode = row[0].ToString();
            //        IncDesc = row[1].ToString();
            //        list.Add(IncCode, IncDesc);
            //    }
            //    e.List = list;
            //}
        }

        private void _form_OnCalledAction(object sender, ActionEventArgs e)
        {
            if (e.Action == "keepIncome")
            {
                //TblPay - is this trying to automate the payment amount??
                //DataTable TblPay = _form.Data.Tables["TblPay"];
                //DataRow rTblPay = TblPay.Rows[1];
                //rTblPay.SetField<bool>(0, true);

                //IField select_row = _form.GetField("TblPay", "select_row");
                //IValidateFieldCommand vSelect_row = _form.Commands.CreateValidateFieldCommand(select_row, rTblPay);
                //vSelect_row.Execute();
            }


        }

        private void _form_OnModelLoad(object sender, ModelLoadEventArgs e)
        {
            ISection recSection = _form.GetSection("SecHeader");
            IField rev_code = recSection.GetField("rev_code");
            rev_code.ControlType = ControlType.DropdownList;
            rev_code.SearchOnDescription = true;
            rev_code.ShowCode = true;
            
        }
    }
}
