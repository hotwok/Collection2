using System.Linq;
using Agresso.Interface.TopGenExtension;
using System.Data;
using Agresso.Interface.CommonExtension;

namespace Collection2
{
    [TopGen("TTS011", "*", "*", "Hourly Cost v1.0")]
    public class TTS011 : IProjectTopGen
    {
        private IForm _form;

        public void Initialize(IForm form)
        {
            _form = form;
            _form.OnModelLoad += _form_OnModelLoad;
            _form.OnLoaded += _form_OnLoaded;
            _form.OnAddingRow += _form_OnAddingRow;
            _form.OnCleared += _form_OnCleared;
            _form.OnValidatedField += _form_OnValidatedField;
        }

        private void _form_OnValidatedField(object sender, ValidateFieldEventArgs e)
        {
            //CurrentContext.Message.Display("Field {0} {1} ", e.FieldName,e.Row.Field<string>("costcategory"));

            if(e.FieldName == "costcategory")
            {
                string Curr = SQLFunc.HCostCurr(e.Row.Field<string>("costcategory"));
                //e.Row.SetField<string>("Currency", Curr);

                //Get the cost cat,look up currency and replace
            }
        }

        private void _form_OnCleared(object sender, ClearEventArgs e)
        {
            AssertData();
        }

        private void _form_OnAddingRow(object sender, AddRowEventArgs e)
        {
            AssertData();
        }

        private void _form_OnLoaded(object sender, LoadEventArgs e)
        {
            //CurrentContext.Message.Display("here2");
            ISection Detail = _form.GetSection("Detail");
            
            IField CostCat = Detail.GetField("costcategory");
            //CostCat.ShowCode = true;
            

            //CurrentContext.Message.Display("here");

            
            if (Detail == null)
                return;

            DataTable HCost = _form.Data.Tables[Detail.TableName];

            if (HCost != null && !HCost.Columns.Contains("Currency"))
            {
                DataColumn newColumn = new DataColumn("Currency", typeof(System.String))
                {
                    DefaultValue = "GBP"
                };

                HCost.Columns.Add(newColumn);
            }

            DataTable DtCostCur =  SQLFunc.HCostDimValue();

            ////foreach (DataRow row in HCost.Rows)
            ////{ 
            ////    row["Curr"] = "GBP";
            ////}



            var updateQuery = from r1 in HCost.AsEnumerable()
                              join r2 in DtCostCur.AsEnumerable()
                              on r1.Field<string>("costcategory") equals r2.Field<string>("CostCat")
                              select new { r1, r2 };
            foreach (var x in updateQuery)
            {
                x.r1.SetField("Currency", x.r2.Field<string>("Cur"));
                
            }
            HCost.AcceptChanges();

        }

        private void _form_OnModelLoad(object sender, ModelLoadEventArgs e)
        {
            //CurrentContext.Message.Display("here");
            ISection Detail = _form.GetSection("Detail");

            IField CostCat = Detail.GetField("costcategory");
            //CostCat.ShowCode = true;
            //CostCat.ControlType = ControlType.DropdownList;
            
            
            IField Status = Detail.GetField("Status");
            //Status.SequenceNo = 15;

            
            if (Detail.GetField("Currency") != null)
                return;
            IField field1 = Detail.Fields.Create("Currency", ControlType.TextBox);
            //field1.SequenceNo = 14;
            field1.IsReadOnly = true;
            field1.Title = "Currency";

            //field1.SequenceNo = 139;
            AssertData();

        }

        void AssertData()
        {
            ISection Detail = _form.GetSection("Detail");

            if (Detail == null)
                return;

            DataTable HCost = _form.Data.Tables[Detail.TableName];

            if (HCost != null && !HCost.Columns.Contains("Currency"))
            {
                DataColumn newColumn = new DataColumn("Currency", typeof(System.String))
                {
                    DefaultValue = "GBP"
                };
                HCost.Columns.Add(newColumn);


                //HCost.Columns.Add("Currency", typeof(string));
            }

        }

    }
}
