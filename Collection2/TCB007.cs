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
    [TopGen("TCB007", "*", "*", "IM Pay Cross Ref - v1.0")]
    public class TCB007 : IProjectTopGen
    {
        private IForm _form;

        public void Initialize(IForm form)
        {
            _form = form;
            _form.OnModelLoad += _form_OnModelLoad;
            _form.OnLoaded += _form_OnLoaded;
        }

        private void _form_OnLoaded(object sender, LoadEventArgs e)
        {
            //CurrentContext.Message.Display("here2");
            ISection Cashbook = _form.GetSection("Cashbook");
            Cashbook.IsResizable = true;
            ISection CashbookTrans = _form.GetSection("CashbookTrans");
            CashbookTrans.IsResizable = true;
            IField cbDescription = CashbookTrans.GetField("description");
            cbDescription.FieldWidth = 2000;
            cbDescription.UsesFullWidth = true;
            //CurrentContext.Message.Display("here");

            ISection StatementTrans = _form.GetSection("StatementTrans");
            IField stDescription = StatementTrans.GetField("description");
            stDescription.FieldWidth = 1000;
            stDescription.DisplayLength = 1000;
        }

        private void _form_OnModelLoad(object sender, ModelLoadEventArgs e)
        {
            //CurrentContext.Message.Display("here");
            ISection CashbookTrans = _form.GetSection("CashbookTrans");
            IField cbDescription = CashbookTrans.GetField("description");
            cbDescription.FieldWidth = 500;
            cbDescription.UsesFullWidth = true;
            //CurrentContext.Message.Display("here");

            ISection StatementTrans = _form.GetSection("StatementTrans");
            IField stDescription = StatementTrans.GetField("description");
            stDescription.FieldWidth = 400;
            stDescription.DisplayLength = 400;



        }
    }
}
