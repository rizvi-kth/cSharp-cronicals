using System;
using System.Windows.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Custom;
using TestStack.White.UIItems.Finders;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Application application = Application.Launch(@"C:\Users\TOOextriha\Documents\Git\cSharp-cronicals\WPF\WhiteTestWPF\WhiteTestWPF\bin\Debug\WhiteTestWPF.exe");
            Window window = application.GetWindow("MainWindow", InitializeOption.NoCache);

            var uiItems = window.GetMultiple(SearchCriteria.ByControlType(ControlType.Custom));
            var uiItems2 = window.Get<UserControl>(SearchCriteria.ByAutomationId("MyParent"));
            uiItems2.DoTheJob();


            Button button = window.Get<Button>("button");
            button.Click();

            //Button button2 = window.Get<Button>("yellowButton");
            //button2.Click();


        }
    }
}
