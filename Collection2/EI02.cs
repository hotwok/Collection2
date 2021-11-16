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
    [Report("EI02", "*", "Error PO Apar id")]
    public class EI02 : IProjectServer
    {
        #region IProjectServer Members
        private IReport _report = null;
        int I = 0;
        int J = 0;
        int K = 0;
        public string tab2;
        public string tab1;
        public void Initialize(IReport report)

        {
            _report = report;
            _report.OnStart += new OnStartEventHandler(OnStart);
            _report.RegisterQueryHandler("EI02_READXML0070", AddAccAnalysis);
            _report.RegisterQueryHandler("EI02_READXML0400", CheckTempTable);
            _report.RegisterQueryHandler("EI02_LIB1350", AddAccounts);
            _report.RegisterQueryHandler("AUTOCHKLIB_0060_OC", DeleteAccts);
            
            _report.OnHelpTableCreated += _report_OnHelpTableCreated1;
            



        }

        void DeleteAccts(object sender, QueryEventArgs e)
        {
            try
            {
                //Here we update uelaeiinvoiceinput with account rule stuff
                //Then delete from tab2 - negative lines
                ++K;

                if (K == 1)
                    {

                    IStatement statement3 = CurrentContext.Database.CreateStatement();

                    statement3.Assign(" database update " + tab2);
                    statement3.Append(" set dim_1 = case ");
                    statement3.Append(" when dim_1 = ' ' then dim_1_a else dim_1 end,");
                    statement3.Append(" dim_2 = case ");
                    statement3.Append(" when dim_2 = ' ' then dim_2_a else dim_2 end ");
                    statement3.Append(" where interface = ' '");

                    CurrentContext.Database.Execute(statement3);


                    IStatement statement1 = CurrentContext.Database.CreateStatement();

                    statement1.Assign(" database UPDATE u ");
                    statement1.Append(" set u.att_1_id = t.att_1_id, ");
                    statement1.Append(" u.att_2_id = t.att_2_id, ");
                    statement1.Append(" u.dim_1    = t.dim_1, ");
                    statement1.Append(" u.dim_2    = t.dim_2 ");
                    statement1.Append(" from uelaeiinvoiceinput u, " + tab2 + " t ");
                    statement1.Append(" where u.account = t.account ");
                    statement1.Append(" and u.client = t.client ");

                    CurrentContext.Database.Execute(statement1);

                    

                    
                    IStatement statement2 = CurrentContext.Database.CreateStatement();

                    statement2.Assign(" database delete " + tab2);
                    statement2.Append(" where interface = ' ' ");

                    CurrentContext.Database.Execute(statement2);
                    }
                _report.API.WriteLog(K + " DELETE XXX");

            }
            catch (Exception ex)
            {
                CurrentContext.Message.Display(((object)ex).ToString());
            }
        }

        void AddAccounts(object sender, QueryEventArgs e)
        {
            try
            {
                _report.API.WriteLog("Check Add accounts, Client =  " + _report.Client.ToString());
                ++J;
                //string tab1 = st;
                
                
                //bool yn = _report.DbAPI.IsTable(st);

                if (J == 1)
                {
                    //Put info from uelaeiinvoiceinput into tab2
                    //Run the account rule update on it
                    IStatement statement1 = CurrentContext.Database.CreateStatement();

                    //statement1.Assign(" database update " + tab1);
                    //statement1.Append(" SET error_flag = error_flag + 16 - 16 * (error_flag / 16 % 2) ");
                    //statement1.Append(" WHERE treat_code = N'1' AND apar_id != apar_id_tmp ");

                    statement1.Assign(" database insert into " + tab2 );
                    statement1.Append(" (account, client, dim_1, att_1_id, att_2_id, account_rule, trans_type, status) ");
                    statement1.Append(" select distinct account, client, dim_1, att_1_id,att_2_id, account_rule,'AP', 'X' from uelaeiinvoiceinput ");
                    statement1.Append(" where client = 'EN' and account != ' ' ");

                    //statement1.SetParameter("client", (object)_form.Client);
                    CurrentContext.Database.Execute(statement1);

                }
                
            }
            catch (Exception ex)
            {
                CurrentContext.Message.Display(((object)ex).ToString());
            }
        }



        void CheckTempTable(object sender, QueryEventArgs e)
        {
            try
            {
                _report.API.WriteLog("Check Temp");
            }
            catch (Exception ex)
            {
                CurrentContext.Message.Display(((object)ex).ToString());
            }
        }




        private void _report_OnHelpTableCreated1(object sender, HelpTableCreatedEventArgs e)
        {
            if (e.Id == "EI02_CRETAB0005")
            {
                CurrentContext.Message.Display("Table Name and table Id {0} - {1}", e.TableName, e.Id);
                tab1 = e.TableName;
            }


            if (e.Id == "EI02_CRETAB0030")
            {
                CurrentContext.Message.Display("Table Name and table Id {0} - {1}", e.TableName, e.Id);
                tab2 = e.TableName;
            }



        }

        void OnStart(object sender, ReportEventArgs e)
        {
            try
            {
                Console.WriteLine("On Start again");

            }
            catch (Exception ex)
            {
                Console.WriteLine("The process failed: {0}", ex.ToString());
            }
        }

        void AddAccAnalysis(object sender, QueryEventArgs e)
        {
            try
            {

                //_report.API.WriteLog("dbxx = '" + _report.DbAPI.AgrDatabase + "'");

                //Code runs twice so only do insert the 1st time - variable I
                ++I;
                //string tab1 = st;
                _report.API.WriteLog(I + " here " + tab2);
                //bool yn = _report.DbAPI.IsTable(st);

                if (I == 1)
                {

                    //test update temp

                    IStatement statement1 = CurrentContext.Database.CreateStatement();

                    //statement1.Assign(" database update " + tab1);
                    //statement1.Append(" SET error_flag = error_flag + 16 - 16 * (error_flag / 16 % 2) ");
                    //statement1.Append(" WHERE treat_code = N'1' AND apar_id != apar_id_tmp ");

                    statement1.Assign(" database insert into Hagrdemom7EI02000500156 ");
                    statement1.Append(" (account, accountable, amount, apar_id, apar_id_tmp, apar_id_valid, apar_type, ");
                    statement1.Append(" archive_ref, arrive_id, arrival_date, art_descr, article, description, ext_inv_ref, ");
                    statement1.Append(" order_id, orig_inv_ref, sequence_no, sup_article, value_1, voucher_no, voucher_ref) ");
                    statement1.Append(" select account, accountable, amount, apar_id, apar_id_tmp, apar_id_valid, apar_type, ");
                    statement1.Append(" archive_ref, arrive_id, arrival_date, art_descr, article, description, ext_inv_ref, ");
                    statement1.Append(" order_id, orig_inv_ref, sequence_no, sup_article, value_1, voucher_no, voucher_ref from " + tab1);

                    
               //statement1.SetParameter("client", (object)_form.Client);
               CurrentContext.Database.Execute(statement1);

                }

            }
            catch (Exception ex)
            {
                CurrentContext.Message.Display(((object)ex).ToString());
            }
        }

        #endregion
    }
}