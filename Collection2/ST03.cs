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
    [View("ST03", "*", "*", "Stock withdrawal kit")]
    internal class ST03 : IProjectForm, IProject
    {
        private IForm _ST03;
        private ISpread _spread;
        private int spRows = 0;

        public void Initialize(IForm iform)
        {
            _ST03 = iform;
            _ST03.OnPostInitialize += new OnPostInitializeEventHandler(_ST03_OnPostInitialize);
        }

        private void _ST03_OnPostInitialize(object sender, ClientEventArgs e)
        {
            IMenuItem GetKits = _ST03.Menu.AppendToolsMenu("KITS");
            GetKits.OnMenuClick += new OnMenuClickEventHandler(GetKits_OnMenuClick);
            _spread = _ST03.GetField("st03tbl1") as ISpread;



        }

        void GetKits_OnMenuClick(object sender, MenuClickEventArgs e)
        {
            DataTable dtOrig = new DataTable();
            dtOrig.Columns.Add("article", typeof(string));
            dtOrig.Columns.Add("value_1", typeof(double));

            foreach (ISpreadRow oR in _spread.Rows)
            {
                //Stop it crashing on the total row
                if (oR.GetValue(1) == "") break;

                DataRow oDR = dtOrig.NewRow();

                oDR["article"] = oR.GetValue("article");
                oDR["value_1"] = double.Parse(oR.GetValue("value_1"));
                dtOrig.Rows.Add(oDR);
            }

            //This gives us where to add the next row
            spRows = dtOrig.Rows.Count + 1;

            //Get all the kits in a new table
            DataTable KitTable1 = SQLFunc.GetKit();

            //New dt to hold the join between kit and product
            DataTable dtst03tbl1 = new DataTable("st03tbl1");

            dtst03tbl1.Columns.Add("product_fx", typeof(string));
            dtst03tbl1.Columns.Add("quantity_fx", typeof(int));
            dtst03tbl1.Columns.Add("value_1", typeof(double));

            var result1 = from dataRows1 in KitTable1.AsEnumerable()
                          join dataRows2 in dtOrig.AsEnumerable()
                          on dataRows1.Field<string>("dim_value") equals dataRows2.Field<string>("article")

                          select dtst03tbl1.LoadDataRow(new object[]
                          {
                                 dataRows1.Field<string>("product_fx"),
                                 dataRows1.Field<int>("quantity_fx"),
                                 dataRows2.Field<double>("value_1")

                         }, false);

            result1.CopyToDataTable();

            //Adds the required number of new rows
            _spread.AddRows(dtst03tbl1.Rows.Count);

            foreach (DataRow dr_dtST03tbl1 in dtst03tbl1.Rows)
            {
                //_spread.SetValue("line_no", spRows, Convert.ToString(spRows));
                _spread.SetValue("article", spRows, dr_dtST03tbl1.Field<string>("product_fx").ToString());
                //Article need to be validated, this adds all the defaults
                //_spread.SetActiveCell("article", spRows);
                //_spread.MoveNextCell();
                //Change the quantity as the above resets it
                //_spread.SetValue("value_1", spRows, Convert.ToString(dr_dtST03tbl1.Field<double>("value_1") * dr_dtST03tbl1.Field<int>("quantity_fx")));
                spRows++;
            }
            //Tidy up to perform quantity validation on the last row
            //_spread.SetActiveCell("line_no", spRows);

        }
    }

}
