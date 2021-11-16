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
    [TopGen("MRC012", "*", "*", "IM Pay Cross Ref - v1.0")]
    public class MRC012 : IProjectTopGen
    {
        private IForm _form;

        public void Initialize(IForm form)
        {
            _form = form;
            _form.OnModelLoad += _form_OnModelLoad;
        }


        private void _form_OnModelLoad(object sender, ModelLoadEventArgs e)
        {
            ISection recSection = _form.GetSection("TblPayCrossRef");
            IField payreference2 = recSection.GetField("payreference2");
            payreference2.IsLockedWhenOld = false;
            //CurrentContext.Message.Display("here");

        }
    }
}
