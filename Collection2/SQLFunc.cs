using Agresso.Interface.CommonExtension;
using System;
using System.Data;

namespace Collection2
{
    internal class SQLFunc
    {
        internal static DataTable GetKit()
        {
            //Return the kit flexi for the ordered Top Part
            IStatement statement = CurrentContext.Database.CreateStatement();
            DataTable dt = new DataTable();
            statement.Assign(" SELECT f.dim_value,f.product_fx, f.quantity_fx, a.article_id, ");
            statement.Append(" a.art_descr, 0 as line_no ");
            statement.Append(" FROM afxsangkit f, algarticle a ");
            //statement.Append(" FROM afxkit f, algarticle a "); //Sanger
            statement.Append(" WHERE f.client = @client");
            statement.Append(" AND a.client = f.client ");
            statement.Append(" AND f.product_fx = a.article ");
            statement.UseAgrParser = true;
            statement["client"] = (object)CurrentContext.Session.Client;
            CurrentContext.Database.Read(statement, dt);
            return dt;
        }

        internal static DataTable ImpRec()
        {
            //Return the kit flexi for the ordered Top Part
            IStatement statement = CurrentContext.Database.CreateStatement();
            DataTable dt = new DataTable();
            statement.Assign(" SELECT distinct receipt_no,  ");
            statement.Append(" a.art_descr, 0 as line_no ");
            statement.Append(" FROM afxsangkit f, algarticle a ");
            //statement.Append(" FROM afxkit f, algarticle a "); //Sanger
            statement.Append(" WHERE f.client = @client");
            statement.Append(" AND a.client = f.client ");
            statement.Append(" AND f.product_fx = a.article ");
            statement.UseAgrParser = true;
            statement["client"] = (object)CurrentContext.Session.Client;
            CurrentContext.Database.Read(statement, dt);
            return dt;
        }

        internal static DataTable CustTrans(string apar_id)
        {
            //Return the kit flexi for the ordered Top Part
            IStatement statement = CurrentContext.Database.CreateStatement();
            DataTable dttrans = new DataTable();
            statement.Assign(" SELECT PCBInvno, voucher_no ");
            statement.Append(" FROM u_am_invoice ");
            statement.Append(" WHERE apar_id = @apar_id");
            statement["apar_id"] = (object)apar_id;
            CurrentContext.Database.Read(statement, dttrans);
            return dttrans;
        }

        //GL15 - end date of the period
        internal static string PeriodEnd(string period)
        {
            IStatement statement = CurrentContext.Database.CreateStatement();
            string EndDate = string.Empty;
            statement.Assign(" DATABASE SELECT Convert(CHAR(8),date_to,112) as date_to ");
            statement.Append(" FROM acrperiod ");
            statement.Append(" WHERE period = @period ");
            //statement.Append(" AND client = 'AM' ");
            statement.Append(" AND client = 'EN' ");
            statement.Append(" AND period_id = 'GL' ");
            statement.Append(" AND status in ('N','P') ");
            statement["period"] = (object)period;
            CurrentContext.Database.ReadValue(statement, ref EndDate);
            if (string.IsNullOrEmpty(EndDate))
                EndDate = "19991231";
            return EndDate;
        }

        //CR29
        internal static string PeriodOpen(string period)
        {
            IStatement statement = CurrentContext.Database.CreateStatement();
            string Open = string.Empty;
            statement.Assign(" SELECT status ");
            statement.Append(" FROM acrperiod ");
            statement.Append(" WHERE period = @period ");
            //statement.Append(" AND client = 'AM' ");
            statement.Append(" AND client = 'EN' ");
            statement.Append(" AND period_id = 'GL' ");
            statement.Append(" AND status in ('N','P') ");
            statement["period"] = (object)period;
            CurrentContext.Database.ReadValue(statement, ref Open);
            if (string.IsNullOrEmpty(Open))
                Open = "C";
            return Open;
        }

        //GL15 - Currency of the client
        internal static string ClientCur(string client)
        {
            //Return the kit flexi for the ordered Top Part
            IStatement statement = CurrentContext.Database.CreateStatement();
            string Cur = string.Empty;
            statement.Assign(" SELECT currency ");
            statement.Append(" FROM acrclient ");
            statement.Append(" WHERE client = @client ");
            statement["client"] = (object)client;
            CurrentContext.Database.ReadValue(statement, ref Cur);
            if (string.IsNullOrEmpty(Cur))
                Cur = string.Empty;
            return Cur;
        }

        internal static DataTable HCostDimValue()
        {

            DataTable HCostCur = new DataTable();
            IStatement statement = CurrentContext.Database.CreateStatement();
            statement.Assign(" SELECT dim_value as CostCat, rel_value as Cur ");
            statement.Append(" FROM agldimvalue ");
            statement.Append(" WHERE client = 'EN' ");
            statement.Append(" AND attribute_id = 'B3' ");
            statement.Append(" AND status = 'N' ");
            statement.Append(" AND rel_value != 'GBP' ");

            CurrentContext.Database.Read(statement, HCostCur);

            return HCostCur;

        }

        internal static string HCostCurr(string dim_value)
        {
            string HCostCur1 = string.Empty;
            IStatement statement = CurrentContext.Database.CreateStatement();
            statement.Assign(" SELECT rel_value ");
            statement.Append(" FROM agldimvalue ");
            statement.Append(" WHERE client = 'EN' ");
            statement.Append(" AND attribute_id = 'B3' ");
            statement.Append(" AND status = 'N' ");
            statement.Append(" and dim_value = @dim_value ");
            statement["dim_value"] = (object)dim_value;

            CurrentContext.Database.ReadValue(statement, ref HCostCur1);

            return HCostCur1;

        }

    }
}
