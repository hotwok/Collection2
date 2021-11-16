using System;
using Agresso.ClientExtension;
using Agresso.Interface.CommonExtension;
using Agresso.ServerExtension;
using System.Data;


namespace Collection2
{
    [View("GL48", "*", "*", "Attribute Master File from link")]
    internal class GL48 : IProjectForm, IProject
    {
        private IForm _GL48;


        public void Initialize(IForm iform)
        {
            _GL48 = iform;
            _GL48.OnPostInitialize += new OnPostInitializeEventHandler(_GL48_OnPostInitialize);
        }

        private void _GL48_OnPostInitialize(object sender, ClientEventArgs e)
        {
            string att_value = CurrentContext.Session.GetProperty("dim_valuex");

            if (att_value != "")
            {
                ICombo dropDownList1 = (ICombo)_GL48.GetField("dim_value");
                dropDownList1.Value = att_value;
                dropDownList1.Validated = false;

                _GL48.ValidateField("dim_value");

                //Clean up so a normal entry of the screen will happen
                att_value = "";
                CurrentContext.Session.SetProperty("dim_valuex", "");
            }

        }
    }

}
