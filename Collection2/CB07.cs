using System;
using Agresso.ClientExtension;
using Agresso.Interface.CommonExtension;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Linq;


namespace Collection2
{
    [View("CB07", "*", "*", "Bank Rec Man Match")]
    internal class CB07 : IProjectForm, IProject
    {
        private IForm _CB07;
        private ISpread _spreadst;
        private ISpread _spreadcb;
              

        public void Initialize(IForm iform)
        {
            _CB07 = iform;
            _CB07.OnPostInitialize += new OnPostInitializeEventHandler(_CB07_OnPostInitialize);         
        }

        
        private void _CB07_OnPostInitialize(object sender, ClientEventArgs e)
        {
            //IMenuItem MarkRow = _CB07.Menu.AppendToolsMenu("MarkAP");
            //MarkRow.OnMenuClick += new OnMenuClickEventHandler(MarkAP_OnMenuClick);
            //_spreadst = _CB07.GetField("cb07tblstmt") as ISpread;
            //_spreadst.SetColVisible(18, true);
            //_spreadst.SetColEnabled(18, true);
            

            //_spreadst.SetValue(18, 2, "test");
            _spreadcb = _CB07.GetField("cb07tblcb") as ISpread;
            //_spreadcb.SetColVisible(18, true);
            _spreadcb.SetColVisible(46, true);

        }
    }

}
