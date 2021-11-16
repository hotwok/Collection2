using Agresso.Interface.CommonExtension;
using Agresso.ServerExtension;

namespace Collection2
{
    [Report("TS03", "*", "TS03 Process Time and Expense - Exch Rate Update v1.0.0.0", -1)]
    public class TS03 : IProjectServer
    {
        private IReport _myReport = (IReport)null;
        int I = 0;


        public void Initialize(IReport report)
        {
            _myReport = report;
            ServerAPI.Current.WriteLog("Initiate TS03 Extension - on Initiate");
            _myReport.RegisterQueryHandler("TS03_COST0060", UpdateTransTable);
            //_myReport.RegisterQueryHandler("TS031030", UpdateforAtstrans);
        }


        private void UpdateTransTable(object sender, QueryEventArgs e)
        {
            ++I;

            if (I == 1)
            {
                IServerAPI api = _myReport.API;
                string[] helpTableNames = _myReport.DbAPI.GetHelpTableNames();
                string empty1 = string.Empty;
                string empty2 = string.Empty;
                int rows = 0;
                ServerAPI.Current.WriteLog("Initiate TS03 Extension - Start");
                ServerAPI.Current.WriteLog("-------------------------------");
                string str1 = helpTableNames.GetValue(15).ToString();
                
                ServerAPI.Current.WriteLog("Start update ... ");
                if (CurrentContext.Database.IsInTransaction())
                    ServerAPI.Current.WriteLog("It is in a transaction");
                
                

                //Get today's exchange rate for each currency to turn into USD
                //Store the Cur rate in 'rate'
                string str2 = " database update t " +
                                " set t.price = t.price * ex.reg_rate " +
                                " from " + str1 + " t, agldimvalue b3, acrexchrates ex " +
                                " where t.costcategory = b3.dim_value " +
                                " and t.client = b3.client " +
                                " and b3.client = ex.client " +
                                " and b3.rel_value = ex.currency " +
                                " and getdate() between ex.date_from and ex.date_to " +
                                " and ex.cur_type = '1' " +
                                " and b3.rel_value not in ('GBP', ' ') ";

                ServerAPI.Current.WriteLog(str2);
                api.DatabaseAPI.ExecSql(str2, "update temporary table with price", ref rows);
                empty1 = string.Empty;
                string str3 = " update " + str1 + " set amount = price * used_hrs ";
                ServerAPI.Current.WriteLog(str3);
                api.DatabaseAPI.ExecSql(str3, "update temporary table with amount ", ref rows);

            }
        }

        //private void UpdateforAtstrans(object sender, QueryEventArgs e)
        //{
        //    ++I;

        //    if (I == 1)
        //    {
        //        IServerAPI api = _myReport.API;
        //        string[] helpTableNames = _myReport.DbAPI.GetHelpTableNames();
        //        string empty1 = string.Empty;
        //        string empty2 = string.Empty;
        //        int rows = 0;
        //        ServerAPI.Current.WriteLog("Initiate TS03 Extension2 - Start");
        //        ServerAPI.Current.WriteLog("-------------------------------");
        //        string str1 = helpTableNames.GetValue(15).ToString();
        //        string str4 = helpTableNames.GetValue(31).ToString();
        //        ServerAPI.Current.WriteLog("Start update2 ... ");
        //        if (CurrentContext.Database.IsInTransaction())
        //            ServerAPI.Current.WriteLog("It is in a transaction2");
        //        //Update atstrans temp with rate in Currency
        //        string str5 = "database update a " +
        //                        " set a.rate = b.rate " +
        //                        " from " + str4 + " a, " + str1 + " b " +
        //                        " where a.client = b.client " +
        //                        " and a.resource_id = b.resource_id " +
        //                        " and a.sequence_no = b.sequence_no " +
        //                        " and a.sequence_ref = b.sequence_ref ";
        //        ServerAPI.Current.WriteLog(str5);
        //        api.DatabaseAPI.ExecSql(str5, "Update atstrans temp with rate in Cur", ref rows);


                //Get today's exchange rate for each currency to turn into USD


        //    }

        //}
    }
}
