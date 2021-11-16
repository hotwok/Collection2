using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Security.AccessControl;
using Agresso.ServerExtension;

using Agresso.ClientExtension;
using Agresso.Interface.CommonExtension;


namespace Collection2
{
    [Report("GL15", "*", "Currency Update", -1)]
    public class GL15 : IProjectServer
    {
        //IProjectServer Members
        private IReport _report = null;
        int I = 0;
        int J = 0;
        int K = 0;
        public string tab1, tab2, tab3, Orig_Account;
        public void Initialize(IReport report)

        {
            _report = report;
            _report.RegisterQueryHandler("AUTOCHKLIB_0080", ErrorAccount);
            _report.RegisterQueryHandler("GL150120", SelectAccounts);
            _report.RegisterQueryHandler("AUTOCHKLIB_0060_OC", AtCursor);
            _report.OnHelpTableCreated += _report_OnHelpTableCreated1;
            _report.OnStart += _report_OnStart;
            

        }

        private void _report_OnStart(object sender, ReportEventArgs e)
        {
            Orig_Account = _report.API.GetParameter("account");
            _report.API.SetParameter("account", "1234");
        }

        private void _report_OnHelpTableCreated1(object sender, HelpTableCreatedEventArgs e)
        {
            if (e.Id == "GL15LIB_0030")
            {
                CurrentContext.Message.Display("ON CREATE Table Name and table Id {0} - {1}", e.TableName, e.Id);
                //cTableGLPost
                tab2 = e.TableName;
            }

            if (e.Id == "GL15LIB_0035")
            {
                CurrentContext.Message.Display("ON CREATE Table Name and table Id {0} - {1}", e.TableName, e.Id);
                //cTableGLAgg
                tab1 = e.TableName;
            }
            
            if (e.Id == "GL150050")
            {
                CurrentContext.Message.Display("ON CREATE Table Name and table Id {0} - {1}", e.TableName, e.Id);
                //cRelVals
                tab3 = e.TableName;
            }
            
        }


        void SelectAccounts(object sender, QueryEventArgs e)
        {

            try
            {
                //_report.API.WriteLog(I + " client1 = " + _report.API.GetParameter("client") + " client2 = " + _report.Client);

                //Code runs twice so only do insert the 1st time - variable I
                ++I;

              
                if (I == 2)
                {
                    _report.API.WriteLog("Orig Account = " + Orig_Account);
                    string PeriodEnd = _report.API.GetParameter("period");

                    string EndDate = SQLFunc.PeriodEnd(PeriodEnd);
                    string ClientCur = SQLFunc.ClientCur(_report.Client);
                    string OpenYear = PeriodEnd.Substring(0, 4) + "00";

                    IStatement statement1 = CurrentContext.Database.CreateStatement();

                    statement1.Assign(" DATABASE INSERT INTO " + tab1);
                    statement1.Append(" ( account , amount , att_1_id , att_2_id, ");
                    statement1.Append(" att_3_id, att_4_id, att_5_id, att_6_id, att_7_id, ");
                    statement1.Append(" calculate_amount, calculate_value_2, calculate_value_3, client,");
                    statement1.Append(" client_currency, cur_amount, currency, dim_1, dim_2, dim_3,");
                    statement1.Append(" dim_4, dim_5, dim_6, dim_7, value_2, value_3, voucher_date, x_voucher_no ) ");
                    statement1.Append(" SELECT t.account , SUM(t.amount) , t.att_1_id , t.att_2_id , ");
                    statement1.Append(" t.att_3_id , t.att_4_id , t.att_5_id , t.att_6_id , t.att_7_id , 1 , ");
                    statement1.Append(" 0 , 0 , t.client , '" + ClientCur + "' , SUM(t.cur_amount) , t.currency , ");
                    statement1.Append(" t.dim_1 , t.dim_2 , t.dim_3 , t.dim_4 , t.dim_5 , t.dim_6 , t.dim_7 , ");
                    statement1.Append(" SUM(t.value_2) , SUM(t.value_3) , convert(datetime,");
                    statement1.Append(" substring('" + EndDate + " 00:00:00', 1, 8) + N' 00:00:00') , ");
                    statement1.Append(" 	 t.voucher_no x_voucher_no ");
                    statement1.Append(" FROM agltransact t , aglaccounts a , " + tab3 + " r ");
                    statement1.Append(" WHERE t.account = r.dim_value and a.account = t.account ");
                    statement1.Append(" and a.account like '" + Orig_Account + "' ");
                    statement1.Append(" AND t.currency = '" + _report.API.GetParameter("currency") + "'");
                    statement1.Append(" AND a.client = 'S1' AND a.client =");
                    //statement1.Append(" AND a.client = 'AM' AND a.client =");
                    statement1.Append(" t.client AND t.period BETWEEN " + OpenYear + " AND " + _report.API.GetParameter("period"));
                    statement1.Append(" GROUP BY t.voucher_no , t.account , t.client , t.currency , ");
                    statement1.Append(" t.att_1_id , t.att_2_id , t.att_3_id , t.att_4_id , t.att_5_id , ");
                    statement1.Append(" t.att_6_id , t.att_7_id , t.dim_1 , t.dim_2 , t.dim_3 , t.dim_4 , ");
                    statement1.Append(" t.dim_5 , t.dim_6 , t.dim_7 ");


                    //statement1.SetParameter("client", (object)_form.Client);
                    CurrentContext.Database.Execute(statement1);
                }

            }
            catch (Exception ex)
            {
                CurrentContext.Message.Display(((object)ex).ToString());
            }
        }

        void AtCursor(object sender, QueryEventArgs e)
        {
            //_report.API.SetParameter

            try
            {
                //Code runs twice so only do insert the 1st time - variable J
                ++K;
                _report.API.WriteLog(K + " here K can cancel = " + e.CanCancel.ToString() );

                if (K == 1)
                {
                    _report.DbAPI.CloseCursor(0);
                    //test update temp

                    IStatement statement1 = CurrentContext.Database.CreateStatement();

                    statement1.Assign("database update " + tab2);
                    statement1.Append(" set noerror = 0, ");
                    statement1.Append(" status = 'N' ");
                    ////statement1.Append(" ,account = '2350'");
                    statement1.Append(" where noerror > 0 ");

                    //statement1.SetParameter("client", (object)_form.Client);
                    CurrentContext.Database.Execute(statement1);

                }
            }
            catch (Exception ex)
            {
                CurrentContext.Message.Display(((object)ex).ToString());
            }
        }



        void ErrorAccount(object sender, QueryEventArgs e)
        {
            e.Cancel = true;
           //_report.API.SetParameter
           
            try
            {
                //Code runs twice so only do insert the 1st time - variable J
                ++J;
                //_report.API.WriteLog(J + " here J can cancel = " + e.CanCancel.ToString() );
               
                if (J == 1)
                {
                   
                    //test update temp

                    IStatement statement1 = CurrentContext.Database.CreateStatement();

                    statement1.Assign("update " + tab2);
                    statement1.Append(" set noerror = 0, ");
                    statement1.Append(" status = 'N' ");
                    //statement1.Append(" ,account = '2350'");
                    statement1.Append(" where noerror > 0 ");

                    //statement1.SetParameter("client", (object)_form.Client);
                    CurrentContext.Database.Execute(statement1);

                }
            }
            catch (Exception ex)
            {
                CurrentContext.Message.Display(((object)ex).ToString());
            }
        }
    }
}