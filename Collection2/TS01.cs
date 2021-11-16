using System;
using Agresso.ClientExtension;
using Agresso.Interface.CommonExtension;
using Agresso.ServerExtension;
using System.Data;


namespace Collection2
{
    [View("TS01", "*", "*", "Project MF - Trigger Flexi")]
    internal class TS01 : IProjectForm, IProject
    {
        private IForm FmTS01;


        public void Initialize(IForm iform)
        {
            FmTS01 = iform;
            FmTS01.OnPostInitialize += FmTS01_OnPostInitialize;
            FmTS01.OnSave += FmTS01_OnSave;
            FmTS01.OnPostSave += FmTS01_OnPostSave;
            
        }

        private void FmTS01_OnPostSave(object sender, PostSaveEventArgs e)
        {
            CurrentContext.Message.Display("save");
            //Check if Project is split
        }

        private void FmTS01_OnSave(object sender, CancelEventArgs e)
        {
            
        }

        private void FmTS01_OnPostInitialize(object sender, ClientEventArgs e)
        {
            CurrentContext.Message.Display("test");

            //IField SiteVisit = _TS47.GetField("site_visit_fx1");
            //IFlexiForm FlexiForm = FmTS01.GetFlexiTabWithSection("AM Project Trigger");

            IMenuItem CreateTrigger = FmTS01.Menu.AppendToolsMenu("Triggers");
            FmTS01.Menu.AppendToolSeparator();
            CreateTrigger.OnMenuClick += CreateTrigger_OnMenuClick;
            
            






            //CurrentContext.Message.Display(FmTS01.HasFlexiTabs.ToString());

        }

        private void CreateTrigger_OnMenuClick(object sender, MenuClickEventArgs e)
        {
            CurrentContext.Message.Display("here");
        }
    }

}
