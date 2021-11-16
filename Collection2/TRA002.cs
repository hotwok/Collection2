using System;
using Agresso.Interface.TopGenExtension;
using Agresso.Interface.CommonExtension;
using System.Data;

namespace Collection2
{
    [TopGen("TRA002", "*", "*", "Own Req change link - v1.0")]
    public class TRA002 : IProjectTopGen
    {
        private IForm _Oform;
        //private ISection detailtable;
        string _supp_flag;



        public void Initialize(IForm form)
        {
            _Oform = form;
            _Oform.OnDataSchemaValidate += _Oform_OnDataSchemaValidate;
            _Oform.OnSystemLinksLoaded += _Oform_OnSystemLinksLoaded;
            _Oform.OnLoaded += _Oform_OnLoaded;
            _Oform.OnCallingAction += _Oform_OnCallingAction;
            _Oform.OnModelLoad += _Oform_OnModelLoad;
            

        }

        private void _Oform_OnModelLoad(object sender, ModelLoadEventArgs e)
        {
            CurrentContext.Message.Display("modal");
        }

        private void _Oform_OnCallingAction(object sender, ActionEventArgs e)
        {
            CurrentContext.Message.Display("call");
        }

        private void _Oform_OnLoaded(object sender, LoadEventArgs e)
        {
            CurrentContext.Message.Display("loaded");
        }

        private void _Oform_OnSystemLinksLoaded(object sender, SystemLinksLoadedEventArgs e)
        {
            CurrentContext.Message.Display("load");

        }

        private void _Oform_OnDataSchemaValidate(object sender, DataSchemaValidateEventArgs e)
        {
            if(_Oform.MenuId == "PO314")
            { 
            ISection Osection = _Oform.GetSection("TemplData_1");
            IField Ftest = Osection.AddField("Adv");
            Ftest.ControlType = ControlType.Button;
            Ftest.ShowDescription = true;
            Ftest.Title = "Req Adv Screen";
            Ftest.VisualInfo = 6;
            Ftest.Tooltip = "Go to Req Adv Screen";
            
            Ftest.Command = "topgen:menu_id=PO282&order_id=:order_id";

            IField Ftest2 = Osection.AddField("PO Screen");
            Ftest2.ControlType = ControlType.Button;
            Ftest2.VisualInfo = 6;
            Ftest2.Title = "PO Screen";
            Ftest2.Tooltip = "Go to PO Screen";
            Ftest2.Command = "topgen:menu_id=PO414&order_id=";

                //IField Oorder = Osection.GetField("test123");
                //IField CalcOrder = Osection.GetField("f0_req2");
                //CalcOrder.ControlType = ControlType.Link;
                //IField Oorder2 = Osection.GetField("order_id2");

                //Oorder.Command = "topgen:menu_id=PO282";

                //CalcOrder.Command = "topgen:menu_id=PO282";
            }
        }
               

       

    }
}
