using System;
using Agresso.Interface.TopGenExtension;
using Agresso.Interface.CommonExtension;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collection2
{
    [TopGen("TPO002", "*", "*", "LGA Req Screen - v1.0")]
    public class TPO002 : IProjectTopGen
    {
        private IForm _Oform;
        //private ISection detailtable;
        IField _supp_flag;
        IField Markings;
        ISection _supp_sec;
        ISection Delivery;


        public void Initialize(IForm form)
        {
            _Oform = form;

            
            _Oform.OnModelLoad += _Oform_OnModelLoad;
            _Oform.OnScreenRefreshed += _Oform_OnScreenRefreshed;
            _Oform.OnValidatedField += _Oform_OnValidatedField;
            _Oform.OnValidatingField += _Oform_OnValidatingField;
            
            
        }

        private void _Oform_OnScreenRefreshed(object sender, ScreenRefreshedEventArgs e)
        {
            _supp_sec = _Oform.GetSection("Default1");
            _supp_sec.SupportsDataDecoration = true;
            _supp_sec.SupportsUIProperties = true;
            _supp_flag = _supp_sec.GetField("fixed_supplier_flag");
            _supp_flag.IsMandatory = true;
            _supp_flag.HasValidation = true;
            _supp_flag.HasValidationOnLeave = true;
            _supp_flag.IsLockedWhenOld = true;

            //Crown
            Delivery = _Oform.GetSection("Delivery");
            Markings = Delivery.GetField("Markings");
            Markings.IsHidden = false;


        }

        private void _Oform_OnValidatingField(object sender, ValidateFieldEventArgs e)
        {
            if (e.FieldName == "fixed_supplier_flag")
            {
                CurrentContext.Message.Display("Here {0}", e.Row["fixed_supplier_flag"].ToString());
                if (e.Row["fixed_supplier_flag"].Equals(false))
                {
                    CurrentContext.Message.Display("false1");
                    _supp_flag.SetFocus();
                    e.Cancel = true;
                    
                }
            }

            if (e.FieldName == "apar_id")
            {
                //e.Row["fixed_supplier_flag"] = true;
                CurrentContext.Message.Display("Here3 {0}", e.Row["fixed_supplier_flag"].ToString());
                if (e.Row["fixed_supplier_flag"].ToString().Equals(false))
                {
                    _supp_flag.SetFocus();
                }
            }

        }

        private void _Oform_OnValidatedField(object sender, ValidateFieldEventArgs e)
        {
            if (e.FieldName == "fixed_supplier_flag")
            {
                CurrentContext.Message.Display("Hed {0}", e.Row["fixed_supplier_flag"].ToString());

            }
        }

        private void _Oform_OnModelLoad(object sender, ModelLoadEventArgs e)
        {
            //_supp_flag = _Oform.GetField("Default1", "fixed_supplier_flag");
            _supp_sec = _Oform.GetSection("Default1");
            _supp_sec.SupportsDataDecoration = true;
            _supp_flag = _supp_sec.GetField("fixed_supplier_flag");
            _supp_flag.IsMandatory = true;
            _supp_flag.HasValidation = true;
            _supp_flag.HasValidationOnLeave = true;
            _supp_flag.IsLockedWhenOld = true;

            //Crown - can we use markings for contract?
            Delivery = _Oform.GetSection("Delivery");

            if (Delivery.GetField("long_info2") != null)
                return;
            IField field1 = Delivery.Fields.Create("long_info2", ControlType.TextBox);
            field1.VisualInfo = 3;
            field1.UsesFullWidth = true;
            field1.HasFieldHelp = false;
            field1.HasValidation = true;
            //field1.IsMandatory = true;
            field1.Title = "Contract";
            //field1.SequenceNo = 139;
            //field1.IsHidden = false;



        }


    }
}
