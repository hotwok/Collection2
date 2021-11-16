using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Agresso.ClientExtension;
using Agresso.Interface.CommonExtension;
using ACT.Common.Data;

namespace Collection2
{
    [View("CR29", "*", "*", "RevalCos Period Check - v1.0.0.0")]
    class CR29 : IProjectForm
    {
        //class variables
        private IForm _cr29;
        private ISpread _spread;

        public void Initialize(IForm iform)
        {
            //This is the right place to take care of the form
            _cr29 = iform;
            //CurrentContext.Message.Display("menu id " + _cr29.MenuId);
            if (_cr29.MenuId != "00101")
                return;

            //and we hook into the suitable event
            _cr29.OnPostInitialize += new OnPostInitializeEventHandler(_cr29_OnPostInitialize);
            _cr29.OnSave += new OnSaveEventHandler(_cr29_OnSave);
        }


        void _cr29_OnSave(object sender, CancelEventArgs e)
        {
            
            for (int i = 9; i <= _spread.RowCount; i++)
            {
                if (_spread.GetString(1, i) == "Period")
                {
                    string Open = SQLFunc.PeriodOpen(_spread.GetString(2, i));
                    if (Open == "C")
                    {
                        CurrentContext.Message.Display("The period is closed");
                        e.Cancel = true;
                    }
                }
            }
        }


        private void _cr29_OnPostInitialize(object sender, ClientEventArgs e)
        {
            //Now we can get the spread
            _spread = _cr29.GetField("cr29tbl1") as ISpread;
            _spread.OnCellValidated += new OnCellValidatedEventHandler(_spread_OnCellValidated);

        }

        void _spread_OnCellValidated(object sender, CellValidatedEventArgs e)
        {
            
            if (e.RowNumber > 10)
            {
                if (_spread.GetString(1,e.RowNumber) == "Period")
                {
                    string Open = SQLFunc.PeriodOpen(_spread.GetString(2, e.RowNumber));
                    //CurrentContext.Message.Display(Open);
                    if (Open == "C")
                    {
                        CurrentContext.Message.Display("The period is closed");
                        e.Cancel = true;
                    }
                }
            }
        }
    }
}
