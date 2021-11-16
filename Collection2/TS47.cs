using System;
using Agresso.ClientExtension;
using Agresso.Interface.CommonExtension;
using Agresso.ServerExtension;
using System.Data;


namespace Collection2
{
    [View("TS47", "*", "*", "Work Order Flexi box")]
    internal class TS47 : IProjectForm, IProject
    {
        private IForm _TS47;


        public void Initialize(IForm iform)
        {
            _TS47 = iform;
            _TS47.OnPostInitialize += _TS47_OnPostInitialize;
        }

        private void _TS47_OnPostInitialize(object sender, ClientEventArgs e)
        {
            CurrentContext.Message.Display("test");
            
            IField SiteVisit = _TS47.GetField("site_visit_fx1");
            
        }
    }

}
