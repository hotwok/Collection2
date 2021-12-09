using System;
using System.Linq;
using Agresso.Interface.TopGenExtension;
using System.Data;
using Agresso.Interface.CommonExtension;

namespace Collection2
{
    [TopGen("TGL048", "*", "*", "Project Screen v1.0")]
    public class TGL048 : IProjectTopGen
    {
        private IForm _form;

        public void Initialize(IForm form)
        {
            _form = form;
            _form.OnLoaded += _form_OnLoaded;
            _form.OnCallingAction += _form_OnCallingAction;
            _form.OnModelLoad += _form_OnModelLoad;
            _form.OnSaving += _form_OnSaving;
            _form.OnValidatedField += _form_OnValidatedField;
           
            
        }

       

        private void _form_OnValidatedField(object sender, ValidateFieldEventArgs e)
        {
            if (e.FieldName == "project_fx")
            { 
            //CurrentContext.Message.Display(e.Row.Field<string>("project_fx").ToString());
            e.Row.SetField<string>("prodesc_fx", e.Row.Field<string>("project_fx").ToString());
            e.Row.SetField<string>("prodesc_fx_descr", e.Row.Field<string>("project_fx_descr").ToString());
            }
        }

                     

        private void _form_OnSaving(object sender, SaveEventArgs e)
        {
            
            ISection SecProj = _form.GetSection("fx_PROJTRIG");
            
            if (SecProj == null)
            { 
                IField FPercent = SecProj.GetField("percent_fx");

                //var test = string.Format("{0:0.00}",_form.Data.Tables[SecProj.TableName.ToString()].Compute($"Sum(percent_fx)", string.Empty)).ToString();
                //var Total100 = Decimal.Parse(_form.Data.Tables[SecProj.TableName.ToString()].Compute($"Sum(percent_fx)", string.Empty).ToString()); 

                Decimal.TryParse(_form.Data.Tables[SecProj.TableName.ToString()].Compute($"Sum(percent_fx)", string.Empty).ToString(), out decimal Total100);

                if (Total100 != 0.00m)
                {
                    e.Cancel = true;
                    CurrentContext.Message.Display("Please check the Total is ZERO - Calculated Total " + Math.Round(Total100,5).ToString("F") + " %");
                }
            }
        }

        private void _form_OnModelLoad(object sender, ModelLoadEventArgs e)
        {
            IActionItem RemoveClear = _form.Menu.GetActionItem(ActionType.Clear);
            RemoveClear.IsHidden = true;

            //IMenuItem HideOpenItems = _form.Menu.Toolbar.GetByName("OpenItems");
            //HideOpenItems.IsHidden = true;

            //Export does not have a menu name! Use iD instead
            //IMenuItem HideExport = _form.Menu.Toolbar.GetById(2);
            //HideExport.IsHidden = true;

        }

        private void _form_OnCallingAction(object sender, ActionEventArgs e)
        {
            if (e.Action.ToString() == "Paste")
            { 
                //clean up the box after pasting onSave??
                DeleteRowsPaste();
            }
        }


        private void _form_OnLoaded(object sender, LoadEventArgs e)
        {
            ISection excelSection = _form.GetSection("ft_PROJTRIGH");
            ISection detailSection = _form.GetSection("fx_PROJTRIG");
            //detailSection.ValidateRows = true;

            if (excelSection != null)
            {
                IField buildDataButton = excelSection.AddField("SplitButton");
                buildDataButton.Title = "Apply";
                buildDataButton.ControlType = ControlType.Button;
                buildDataButton.Command = "action:" + "Paste";
            }

            if (detailSection != null)
            {
                IField Project = detailSection.GetField("prodesc_fx");
                //Project.IsReadOnly = true;
                
            
            }
            //_form.Data.Tables[14].RowChanging += TGL048_RowChanging;
            

        }

        
        void AssertData()
        {


        }

        void DeleteRowsPaste()
        {
            DataTable dtSplit = _form.Data.Tables["afxprojtrig"];
            DataTable dtHead = _form.Data.Tables["afxprojtrigh"];

            string stPasted = dtHead.Rows[0].Field<string>("paste_fx").ToString();

            //If we paste twice it crashes
            dtSplit.AcceptChanges();

            foreach (DataRow drow in dtSplit.Rows)
            {
                drow.Delete();//Mark a row for deletion.
            }
            dtSplit.Clear();
            dtSplit.AcceptChanges();

            string[] lines = stPasted.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int i = 0;
            foreach (var line in lines)
            {
                string[] split = line.Split('\t');

                DataRow row = dtSplit.NewRow();

                row.SetField("line_no", i);
                ++i;
                row.SetField("attribute_id", "Z002"); //and this
                row.SetField("dim_value", "LONSPLIT");//get this from the header
                row.SetField("client", "EN"); //and this
                //look up active fields on flexi (section?)
                row.SetField("project_fx", split[0]);
                row.SetField("prodesc_fx", split[0]);
                row.SetField("percent_fx", decimal.Parse(split[1]));

                dtSplit.Rows.Add(row);
             
            }
            //dtSplit.GetChanges();
            //dtSplit.AcceptChanges();
            //Monday
            //issue with saving rows in error if acceptchanges() or getting double rows
            //saved if acceptchanges is off

        }

    }
}
