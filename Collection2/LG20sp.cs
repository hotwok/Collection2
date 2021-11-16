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
    [Report("LG20", "*", "Contract Details")]
    public class LG20 : IProjectServer
    {
        #region IProjectServer Members
        private IReport _report = null;
        int I = 0;
        public string tab2;
        public string tab1;
        public void Initialize(IReport report)
            
        {
            _report = report;
            _report.OnStart += new OnStartEventHandler(OnStart);
            _report.RegisterQueryHandler("LG201330", AddAccAnalysis);
            _report.OnHelpTableCreated += _report_OnHelpTableCreated1;
            
            

        }

        private void _report_OnHelpTableCreated1(object sender, HelpTableCreatedEventArgs e)
        {
            if (e.Id == "LG201375")
            {
                CurrentContext.Message.Display("Table Name and table Id {0} - {1}", e.TableName, e.Id);
                tab2 = e.TableName;
            }

            if (e.Id == "LG200060")
            {
                CurrentContext.Message.Display("Table Name and table Id {0} - {1}", e.TableName, e.Id);
                tab1 = e.TableName;
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

                if(I == 1)
                {
                              
                //test update temp
                        
                IStatement statement1 = CurrentContext.Database.CreateStatement();

                    statement1.Assign(" database update a ");
                    statement1.Append(" set a.dim_1 = b.rel_value1, ");
                    statement1.Append(" a.dim_2 = b.rel_value2, ");
                    statement1.Append(" a.dim_3 = b.rel_value3, ");
                    statement1.Append(" a.dim_4 = b.rel_value4, ");
                    statement1.Append(" a.dim_5 = b.rel_value5, ");
                    statement1.Append(" a.dim_6 = b.text1, ");
                    statement1.Append(" a.dim_7 = b.text2, ");
                    statement1.Append(" a.att_1_id = r.att_1_id, ");
                    statement1.Append(" a.att_2_id = r.att_2_id, ");
                    statement1.Append(" a.att_3_id = r.att_3_id, ");
                    statement1.Append(" a.att_4_id = r.att_4_id, ");
                    statement1.Append(" a.att_5_id = r.att_5_id, ");
                    statement1.Append(" a.att_6_id = r.att_6_id, ");
                    statement1.Append(" a.att_7_id = r.att_7_id ");
                    statement1.Append(" from " + tab2 + " a," + tab1 + " b, aglrules r, aglaccounts g ");
                    statement1.Append(" where a.client = 'EN' ");
                    statement1.Append(" and a.client = b.client ");
                    statement1.Append(" and a.new_date_from = b.date_from ");
                    statement1.Append(" and a.new_date_to = b.date_to ");
                    statement1.Append(" and a.article = b.article ");
                    statement1.Append(" and a.contract_id = b.contract_id ");
                    statement1.Append(" and r.client = a.client ");
                    statement1.Append(" and r.account_rule = g.account_rule ");
                    statement1.Append(" and g.account = a.account ");
                    statement1.Append(" and r.client = g.client ");
                    statement1.Append(" and g.status = 'N' ");


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