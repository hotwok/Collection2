using System;
using Agresso.Interface.TopGenExtension;
using Agresso.Interface.CommonExtension;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collection2
{
    [TopGen("TCR022", "*", "*", "SOU Form Clear button- v1.0")]
    public class TCR022 : IProjectTopGen
    {
        private IForm Oform;
        //private ISection detailtable;
        //IField _supp_flag;
        //ISection _supp_sec;


        public void Initialize(IForm form)
        {
            Oform = form;


            Oform.OnModelLoad += _Oform_OnModelLoad;
            Oform.OnScreenRefreshed += _Oform_OnScreenRefreshed;
        }

        private void _Oform_OnScreenRefreshed(object sender, ScreenRefreshedEventArgs e)
        {
            IActionItem RemoveClear = Oform.Menu.GetActionItem(ActionType.Clear);
            RemoveClear.IsHidden = true;
        }
               
        private void _Oform_OnModelLoad(object sender, ModelLoadEventArgs e)
        {
            IActionItem RemoveClear = Oform.Menu.GetActionItem(ActionType.Clear);
            RemoveClear.IsHidden = true;
        }


    }
}
