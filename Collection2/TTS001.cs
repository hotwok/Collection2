using System.Linq;
using Agresso.Interface.TopGenExtension;
using System.Data;
using Agresso.Interface.CommonExtension;

namespace Collection2
{
    [TopGen("TTS001", "*", "*", "Project Screen v1.0")]
    public class TTS001 : IProjectTopGen
    {
        private IForm _form;

        public void Initialize(IForm form)
        {
            _form = form;
            _form.OnModelLoad += _form_OnModelLoad;
            _form.OnLoaded += _form_OnLoaded;
            _form.OnCallingAction += _form_OnCallingAction;
            _form.OnAddingRow += _form_OnAddingRow;
            _form.OnCleared += _form_OnCleared;
        
        }

        private void _form_OnCallingAction(object sender, ActionEventArgs e)
        {
            CurrentContext.Message.Display("Button pressed {0}",e.Action.ToString());
        }

        private void _form_OnCleared(object sender, ClearEventArgs e)
        {
        //    AssertData();
        }

        private void _form_OnAddingRow(object sender, AddRowEventArgs e)
        {
            //AssertData();
        }

        private void _form_OnLoaded(object sender, LoadEventArgs e)
        {
            //_form.Menu.Create("Action:Test", MenuType.ToolsMenu);
            //IMenuItem CreateTrigger = _form.Menu.Create("Action:Test", MenuType.ToolsMenu);
            //CreateTrigger.MenuName = "test2";
            
        }

        private void _form_OnModelLoad(object sender, ModelLoadEventArgs e)
        {
            //IMenuItem sub = _form.Menu.ToolsMenu.GetSubMenuAt(0, 0);

            IMenuItem CreateTrigger = _form.Menu.Create("action:Test", MenuType.ToolBar);
            CreateTrigger.MenuName = "test2";
            CreateTrigger.Title = "test3";

            ISection headerSection = _form.GetSection("ft_PROJTRIGH");

            IField invoiceableButton = headerSection.Fields.Create("TestButton", ControlType.Button);
            invoiceableButton.Title = "test3";
            invoiceableButton.Command = "action:Test";
            invoiceableButton.IsReadOnly = false;
            invoiceableButton.IsHidden = false;
            
        }
    

        void AssertData()
        {
        

        }

    }
}
