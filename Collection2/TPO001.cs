using System;
using Agresso.Interface.TopGenExtension;
using Agresso.Interface.CommonExtension;
using System.Data;
using System.Linq;

namespace Collection2
{
    [TopGen("TPO001", "*", "*", "Req Adv Screen - v1.0")]
    public class TPO001 : IProjectTopGen
    {
        private IForm _Oform;
        //private ISection detailtable;
        string _supp_flag;
        IField field1;
        IField field2;
        IField field3;
        IField field4;
        ISection Default1;


        public void Initialize(IForm form)
        {
            _Oform = form;
            _Oform.OnSaving += _Oform_OnSaving;
            _Oform.OnModelLoad += _Oform_OnModelLoad;
            _Oform.OnScreenRefreshed += _Oform_OnScreenRefreshed;
            _Oform.OnValidatedField += _Oform_OnValidatedField;
        }

        private void _Oform_OnValidatedField(object sender, ValidateFieldEventArgs e)
        {
            if (e.FieldName == "long_info2")
            {
                //CurrentContext.Message.Display(e.Row["long_info2"].ToString());
                if (string.IsNullOrEmpty(e.Row["long_info2"].ToString()) == true)
                {
                    CurrentContext.Message.Display(MessageDisplayType.Error, "Please enter a contract or N for No Contract");
                    e.Cancel = true;
                    field1.SetFocus();

                }

                if ( e.Row["long_info2"].ToString().ToUpper() == "N")
                {
                    e.Row["long_info2"] = "No contract";

                }


            }
        }

        private void _Oform_OnScreenRefreshed(object sender, ScreenRefreshedEventArgs e)
        {
            //Crown - can we use markings for contract?
            Default1 = _Oform.GetSection("default");

            if (Default1.GetField("long_info2") != null)
                return;
            IField field1 = Default1.Fields.Create("long_info2", ControlType.TextBox);
            
            field1.UsesFullWidth = true;
            field1.HasFieldHelp = false;
            field1.HasValidation = true;
            //field1.HasValidationOnLeave = true;
            field1.IsMandatory = true;
            field1.Title = "Contract";
            //field1.SequenceNo = 139;
            //field1.IsHidden = false;
            field1.FieldWidth = 12;
            field1.DisplayLength = 12;

            

        }

        private void _Oform_OnModelLoad(object sender, ModelLoadEventArgs e)
        {
            //Crown - can we use markings for contract?
            Default1 = _Oform.GetSection("default");

            if (Default1.GetField("long_info2") != null)
                return;
            field1 = Default1.Fields.Create("long_info2", ControlType.TextBox);
            
            //field1.UsesFullWidth = true;
            field1.HasFieldHelp = false;
            field1.HasValidation = true;
            //field1.HasValidationOnLeave = true;
            field1.Title = "Contract";
            //field1.FieldWidth = 12;
            //field1.DisplayLength = 12;


            if (Default1.GetField("con_text") != null)
                return;
            field2 = Default1.Fields.Create("con_text", ControlType.Label);
            field2.IsReadOnly = true;
            field2.Title = "If contract related please refer to contracts database to";
            //field2.UsesFullWidth = true;

            //if (Default1.GetField("con_text1") != null)
            //    return;
            //field3 = Default1.Fields.Create("con_text1", ControlType.Label);
            //field3.IsReadOnly = true;
            //field3.Title = "15               ";

            if (Default1.GetField("con_text2") != null)
                return;
            field4 = Default1.Fields.Create("con_text2", ControlType.Label);
            field4.IsReadOnly = true;
            field4.Title = "determine contract reference otherwise enter N for no contract.";



        }

        private void _Oform_OnSaving(object sender, SaveEventArgs e)
        {
            DataRow _rowN = _Oform.Data.Tables["aporeqheader"].Rows[0];
            _supp_flag = _rowN["fixed_supplier_flag"].ToString();           

            if (_supp_flag.Equals("False"))
                {
                _rowN.SetColumnError("fixed_supplier_flag", "Please tick the box.");
                e.Cancel = true;
                }
        }

    }
}
