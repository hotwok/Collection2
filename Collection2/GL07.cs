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
    [Report("GL07", "*", "Allow AP/AR", -1)]
    public class GL07 : IProjectServer
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
            _report.RegisterQueryHandler("AUTOCHKLIB_0060_OC", AtCursor);
            _report.OnHelpTableCreated += _report_OnHelpTableCreated1;
        
        }

        
        private void _report_OnHelpTableCreated1(object sender, HelpTableCreatedEventArgs e)
        {
            if (e.Id == "Tables_0030")
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

        void AtCursor(object sender, QueryEventArgs e)
        {
            //_report.API.SetParameter

            try
            {
                //Code runs twice so only do insert the 1st time - variable J
                ++K;
                _report.API.WriteLog(K + " here K can cancel = " + e.CanCancel.ToString());

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

    }
}