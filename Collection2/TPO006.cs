using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ACT.Common.Data;
using Agresso.Interface.CommonExtension;
using Agresso.Interface.TopGenExtension;
using System.Data;



namespace Collection2
{
    [TopGen("TPO006", "*", "*", "Sanger Goods Received")]
    public class TPO006 : IProjectTopGen
    {
        private IForm _form;
        private string Storage_Type = string.Empty;
        private bool update;


        public void Initialize(IForm form)
        {
            _form = form;
            _form.OnModelLoad += new OnModelLoadEventHandler(_form_OnModelLoad);
            _form.OnCleared += new OnClearEventHandler(_form_OnCleared);
            _form.OnLoaded += new OnLoadEventHandler(_form_OnLoaded);
            _form.OnValidatedField += new OnValidateFieldEventHandler(_form_OnValidatedField);
            _form.OnGetValueList += new EventHandler<ValueListEventArgs>(_form_OnGetValueList);
            _form.OnSaved += new OnSaveEventHandler(_form_OnSaved);
            _form.OnSaving += new OnSaveEventHandler(_form_OnSaving);
        }

        private void _form_OnSaving(object sender, SaveEventArgs e)
        {
            if (_form.Platform == 1)
            { 
            DataRow[] _rowN = _form.Data.Tables["algdelivery"].Select("received_val > 0");

                //If just a return is done there are no rows delivered so it will crash finding a voucher
                if (_rowN.Count() > 0)
                { 
                string VT = _rowN[0]["voucher_type"].ToString();
                update = true;

                    if (VT == "IE" || VT == "IN" || VT == "IS" || VT == "IV")
                    {
                        update = false;
                    }
                }

                if (update)
                {

                    for (int i = 0; i < _rowN.Count(); i++)
                    {
                        //Check Storage_Type
                        string chkST = _rowN[i]["Storage_Type"].ToString();

                        if (string.IsNullOrWhiteSpace(chkST))
                        {
                            //CurrentContext.Message.Display("Storage Type is Mandatory");
                            _rowN[i].SetColumnError("Storage_Type", " is mandatory");
                            //_rowN[i].SetField("Storage_Type", "XXXXXX");
                            e.Cancel = true;
                        }
                    }
                }
            }
        }

        private void _form_OnGetValueList(object sender, ValueListEventArgs e)
        {
            if (e.FieldName == "Storage_Type")
            {
                IValueDescriptionList titleList1 =
                ValueDescriptionListFactory.Create<string>();

                titleList1.Add("Ambient", "Ambient");
                titleList1.Add("Equipment", "Equipment");
                titleList1.Add("Fridge +4", "Fridge +4");
                titleList1.Add("Freezer -20", "Freezer -20");
                titleList1.Add("Freezer -80", "Freezer -80");
                titleList1.Add("LN2", "LN2");
                e.List = titleList1;
            }

        }

        void _form_OnValidatedField(object sender, ValidateFieldEventArgs e)
        {
            if (e.SectionName == "order_details" && e.FieldName == "Storage_Type")
            {
                Storage_Type = (string)e.Row["Storage_Type"];
            }
        }


        void _form_OnCleared(object sender, ClearEventArgs e)
        {
            AssertData();
        }


        void _form_OnLoaded(object sender, LoadEventArgs e)
        {
            AssertData();
        }

        void AssertData()
        {
            ISection detail = _form.GetSection("order_details");
            if (detail == null)
                return;

            DataTable dt = _form.Data.Tables[detail.TableName];

            if (dt != null && !dt.Columns.Contains("Storage_Type"))
                dt.Columns.Add("Storage_Type", typeof(string));

        }

        void _form_OnSaved(object sender, SaveEventArgs e)
        {
            if (_form.Platform == 1 && update == true)
            { 
                if (!e.Cancel)
                {
                    //Do the headers based on Row 0
                    DataRow[] _rowN = _form.Data.Tables["algdelivery"].Select("arrive_id > 0");

                    DataRow _rowN1 = _rowN[0];

                    //Dim value description - Row 0
                    IStatement statement = CurrentContext.Database.CreateStatement();
                    {

                        statement.Assign("insert into agldescription ");
                        statement.Append("(client, ");
                        statement.Append("attribute_id, ");
                        statement.Append("dim_value, ");
                        statement.Append("description, ");
                        statement.Append("language) ");
                        statement.Append("values ( ");
                        statement.Append("@client, ");
                        statement.Append(" '14', ");
                        statement.Append("@arrive_id, ");
                        statement.Append("@description, ");
                        statement.Append("'EN')");
                        statement["client"] = _form.Client;
                        statement["arrive_id"] = _rowN1["arrive_id"].ToString();
                        statement["description"] = _rowN1["apar_id"].ToString();

                        CurrentContext.Database.Execute(statement);

                    }
                    //Gin Record - dim value Row 0
                    IStatement statement1 = CurrentContext.Database.CreateStatement();
                    {

                        statement1.Assign("insert into agldimvalue ");
                        statement1.Append("(client, ");
                        statement1.Append("attribute_id, ");
                        statement1.Append("dim_value, ");
                        statement1.Append("description, ");
                        statement1.Append("last_update, ");
                        statement1.Append("period_from, ");
                        statement1.Append("period_to, ");
                        statement1.Append("status, ");
                        statement1.Append("user_id) ");
                        statement1.Append("values ( ");
                        statement1.Append("@client, ");
                        statement1.Append(" '14', ");
                        statement1.Append("@arrive_id, ");
                        statement1.Append("@description, ");
                        statement1.Append(" getdate(), ");
                        statement1.Append("0, ");
                        statement1.Append("209912, ");
                        statement1.Append("'N', ");
                        statement1.Append("'SYSEN')");
                        statement1["client"] = _form.Client;
                        statement1["arrive_id"] = _rowN1["arrive_id"].ToString();
                        statement1["description"] = _rowN1["apar_id"].ToString();

                        CurrentContext.Database.Execute(statement1);

                    }

                    //Attribute Master file Flex GIN - Row 0
                    IStatement statement2 = CurrentContext.Database.CreateStatement();
                    {
                        statement2.Assign("insert into afxgin ");
                        statement2.Append("(client, ");
                        statement2.Append("attribute_id, ");
                        statement2.Append("dim_value, ");
                        statement2.Append("line_no, ");
                        statement2.Append("order_no, ");
                        statement2.Append("date_arrived, ");
                        statement2.Append("date_of_gin, ");
                        statement2.Append("last_update, ");
                        statement2.Append("user_id)");
                        statement2.Append("values ( ");
                        statement2.Append("@client, ");
                        statement2.Append(" '14', ");
                        statement2.Append("@arrive_id, ");
                        statement2.Append("0, ");
                        statement2.Append("@order_id, ");
                        statement2.Append(" getdate(), ");
                        statement2.Append(" getdate(), ");
                        statement2.Append(" getdate(), ");
                        statement2.Append("'SYSEN')");
                        statement2["client"] = _form.Client;
                        statement2["arrive_id"] = _rowN1["arrive_id"].ToString();
                        statement2["order_id"] = _rowN1["order_id"].ToString();

                        CurrentContext.Database.Execute(statement2);
                    }

                    //Attribute Master file Flex GINSTORCOND - distinct rows
                    //Get Distinct rows by storage type
                    DataTable dtd = _form.Data.Tables["algdelivery"];

                    //Delete the rows not delivered
                    var rows = dtd.Select("arrive_id = 0");
                    foreach (var row in rows)
                        row.Delete();

                    //Delete negative rows
                    var rows1 = dtd.Select("vow_amount =< 0");
                    foreach (var row1 in rows1)
                        row1.Delete();

                    //Update the table for the deleted rows
                    dtd.AcceptChanges();

                    DataView view = new DataView(dtd);
                    DataTable dValues = view.ToTable(true, "Storage_Type");

                    for (int i = 0; i < dValues.Rows.Count; i++)
                    {
                        DataRow dr = dValues.Rows[i];
                        IStatement statement3 = CurrentContext.Database.CreateStatement();
                        {
                            statement3.Assign("insert into afxginstorcond ");
                            statement3.Append("(client, ");
                            statement3.Append("attribute_id, ");
                            statement3.Append("dim_value, ");
                            statement3.Append("line_no, ");
                            statement3.Append("yellow_label_no, ");
                            statement3.Append("storge_type, ");
                            statement3.Append("supplier, ");
                            statement3.Append("last_update, ");
                            statement3.Append("user_id)");
                            statement3.Append("values ( ");
                            statement3.Append("@client, ");
                            statement3.Append(" '14', ");
                            statement3.Append("@arrive_id, ");
                            statement3.Append("@row, ");
                            statement3.Append("@row + 1, ");
                            statement3.Append("@Storage_Type, ");
                            statement3.Append("@apar_id, ");
                            statement3.Append(" getdate(), ");
                            statement3.Append("'SYSEN')");
                            statement3["client"] = _form.Client;
                            statement3["arrive_id"] = _rowN1["arrive_id"].ToString();
                            statement3["row"] = i;
                            statement3["Storage_Type"] = dr[columnName: "Storage_Type"].ToString();
                            statement3["apar_id"] = _rowN1["apar_id"].ToString();

                            CurrentContext.Database.Execute(statement3);
                        }
                        //Set a fake property and pick it up in attribute master file
                        CurrentContext.Session.SetProperty("dim_valuex", _rowN1["arrive_id"].ToString());
                    }
                }
            }


        }

        void _form_OnModelLoad(object sender, ModelLoadEventArgs e)
        {
            ISection section = this._form.GetSection("order_details");

            if (section.GetField("Storage_Type") != null)
                return;
            IField field1 = section.Fields.Create("Storage_Type", ControlType.DropdownList);
            field1.VisualInfo = 3;
            field1.UsesFullWidth = true;
            field1.HasFieldHelp = false;
            field1.HasValidation = true;
            //field1.IsMandatory = true;
            field1.Title = "Storage Type";
            field1.SequenceNo = 139;
            field1.IsHidden = false;
            

        }
    }

}