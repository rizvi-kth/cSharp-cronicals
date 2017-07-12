using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.Factory;
using TestStack.White.UIItems;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Application application = Application.Launch(@"C:\Users\A547184\Documents\Visual Studio 2015\Projects\WhiteTestWPF\WhiteTestWPF\bin\Debug\WhiteTestWPF");
            Window window = application.GetWindow("MainWindow", InitializeOption.NoCache);

            Button button = window.Get<Button>("button");
            button.Click();

            Button button2 = window.Get<Button>("yellowButton");
            button2.Click();


        }
    }
}
