using Agresso.Interface.TopGenExtension;
using Agresso.Interface.CommonExtension;
using System.Data;
using ACT.Common.Data;


namespace Collection2
{
    [TopGenAttribute("TRA015", "1022", "*", "RevalCos Check Period - v1.0.0.0")]
    public class TRA015_RevP : IProjectTopGen
    {
        IForm _form;
        string Open = "C";


        public void Initialize(IForm form)
        {
            _form = form;
            _form.OnValidatedField += new OnValidateFieldEventHandler(Validated);
            _form.OnScreenRefreshed += _form_OnScreenRefreshed;
            _form.OnGetValueList += _form_OnGetValueList;
            _form.OnModelLoad += _form_OnModelLoad;
        }

        private void _form_OnModelLoad(object sender, ModelLoadEventArgs e)
        {
            IField Period = _form.GetField("OpenParams", "period");
            if (Period != null)
            {
                Period.ControlType = ControlType.DropdownList;



            }
        }

        private void _form_OnGetValueList(object sender, ValueListEventArgs e)
        {
            IValueDescriptionList list = ValueDescriptionListFactory.Create<string>();
            //A1 = Tax code

            if(e.FieldName == "period")
            { 
                IStatement sql = CurrentContext.Database.CreateStatement();
                sql.Assign(" SELECT period ");
                sql.Append(" FROM acrperiod ");
                sql.Append(" WHERE client='EN' ");
                sql.Append(" AND status in ('N','P') ");
                sql.Append(" AND period_id = 'GL' ");
                sql.Append(" ORDER BY period desc ");
                
                DataTable dt = new DataTable();
                CurrentContext.Database.Read(sql, dt);

                string OPeriod = "";
                foreach (DataRow row in dt.Rows)
                {
                    OPeriod = row[0].ToString();
                
                    list.Add(OPeriod, OPeriod);
                }
                e.List = list;
            }
        }

        private void _form_OnScreenRefreshed(object sender, ScreenRefreshedEventArgs e)
        {
            IField Period = _form.GetField("OpenParams", "period");
            if (Period != null)
            {
                Period.ControlType = ControlType.DropdownList;

                IValueDescriptionList list = ValueDescriptionListFactory.Create<string>();
                //A1 = Tax code

                    IStatement sql = CurrentContext.Database.CreateStatement();
                    sql.Assign(" SELECT period ");
                    sql.Append(" FROM acrperiod ");
                    sql.Append(" WHERE client='EN' ");
                    sql.Append(" AND status in ('N','P') ");
                    sql.Append(" AND period_id = 'GL' ");
                    sql.Append(" ORDER BY period desc ");

                    DataTable dt = new DataTable();
                    CurrentContext.Database.Read(sql, dt);

                    string OPeriod = "";
                    foreach (DataRow row in dt.Rows)
                    {
                        OPeriod = row[0].ToString();

                        list.Add(OPeriod, OPeriod);
                    }
                    Period.SetList(list);



            }
        }

        

        void Validated(object sender, ValidateFieldEventArgs e)
        {
            
            if ((e.SectionName == "OpenParams") && (e.FieldName == "period"))
            {
                DataRow PRow = _form.Data.Tables["repparamdata"].Rows[0];
                Open = SQLFunc.PeriodOpen(PRow["period"].ToString());

                if (Open == "C")
                {
                    CurrentContext.Message.Display("The period is closed");
                    e.Cancel = true;
                    PRow.SetColumnError("period", "Period is Closed");
                    PRow.RowError = "Error, Period Closed";
                    //PRow.RejectChanges();
                    IField FPeriod = _form.GetField("OpenParams", "period");
                    FPeriod.SetFocus();
                }
 
            }
        }

    }
}