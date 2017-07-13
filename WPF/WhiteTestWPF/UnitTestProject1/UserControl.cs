using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using TestStack.White.UIA;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Actions;
using TestStack.White.UIItems.Custom;

namespace UnitTestProject1
{
    [ControlTypeMapping(CustomUIItemType.Custom, WindowsFramework.Wpf)]
    public class UserControl : CustomUIItem
    {
        public UserControl(
            AutomationElement automationElement,
            ActionListener actionListener)
            : base(automationElement, actionListener)
        {
        }

        protected UserControl()
        {
        }

        public virtual void DoTheJob()
        {
            //Base class, i.e. CustomUIItem has property called Container. Use this find the items within this.
            //Can also use SearchCriteria for find items
            Button button2 = Container.Get<Button>("yellowButton");
            button2.Click();

            //Container.Get<TextBox>("day").Text = dateTime.Day.ToString();


            //Container.Get<TextBox>("month").Text = dateTime.Month.ToString();
            //Container.Get<TextBox>("year").Text = dateTime.Year.ToString();
        }
    }
}
