using System;
using Agresso.ClientExtension;
using Agresso.Interface.CommonExtension;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Linq;


namespace Collection2
{
    [View("TS11", "*", "*", "Cost Cat Currency")]
    internal class TS11 : IProjectForm, IProject
    {
        private IForm FmTS11;
        private ISpread SpQrytbl1;
        private IField act_curr;
        //private ISpread spreadcb;


        public void Initialize(IForm iform)
        {
            FmTS11 = iform;
            FmTS11.OnFieldFocusChange += FmTS11_OnFieldFocusChange;
            //CB07.OnPostInitialize += new OnPostInitializeEventHandler(_CB07_OnPostInitialize);
            FmTS11.OnPostInitialize += FmTS11_OnPostInitialize;
            
        }

        private void FmTS11_OnFieldFocusChange(object sender, FocusChangeEventArgs e)
        {
            //CurrentContext.Message.Display(e.SourceID);
            if (e.SourceID == "description")
            { 
                act_curr = FmTS11.GetField("act_Currency");
                act_curr.Value = "GBP";
                act_curr.Enabled = false;
                
                FmTS11.MoveFocusNext();
            }
        }

        private void FmTS11_OnPostInitialize(object sender, ClientEventArgs e)
        {
            IFieldBuilder field1 = new FieldBuilder(FmTS11, "Currency")
            {
                 DataType = DynamicFieldDataType.String,
                 FieldType = DynamicFieldType.Edit,
                 //FieldType = DynamicFieldType.Label,
                 
                 Width = 50
                   
                 
            };

            
            field1.PlaceAfter(FmTS11.GetField("description"));
            field1.TabAfter = "description";

            FmTS11.AddField(field1);

            //_spreadst.SetValue(18, 2, "test");
            //SpQrytbl1 = FmTS11.GetField("qrytbl1") as ISpread;
            ////SpQrytbl1.CreateColumn();

            //IColumnBuilder columnBuilder = new ColumnBuilder("CurrCol")
            //{
            //    ColumnType =  DynamicColumnType.Text,
            //    DefaultValue = "GBP",
            //    Title = "Currency",
            //    ID = "Currency"
                     
            //};

            //SpQrytbl1.AddColumn(columnBuilder);
            //SpQrytbl1.SetColEnabled(33, false);
            //SpQrytbl1.SetColEnabled(34, false);


            
            
            

        }
    }

}
