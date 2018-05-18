using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ElementInteractions
{
    [TestClass]
    public class TestScenarios
{

        static IWebDriver Driver;
        public IWebElement Table { get; private set; }
        private  int columnIndex = 0, rowIndex = 0;

        [TestInitialize]

    public void BrowserInitialise()
    {
            Driver = new ChromeDriver();   

    }

    [TestMethod]
    [TestCategory("Navigation")]

    public void PracticeTest()
    {
            //Driver.Navigate().GoToUrl("http://www.ultimateqa.com");
            //Driver.Manage().Window.Maximize();


            //Assert.AreEqual("Home - Ultimate QA", Driver.Title);

            //Driver.Navigate().GoToUrl("https://www.ultimateqa.com/automation");
            //Assert.AreEqual("Automation Practice - Ultimate QA", Driver.Title);

            //Driver.FindElement(By.XPath("//*[@href='../complicated-page']")).Click();

            //Assert.AreEqual("Complicated Page - Ultimate QA", Driver.Title);

            //Driver.Navigate().Back();

            //Assert.AreEqual("Automation Practice - Ultimate QA",Driver.Title);

            Driver.Navigate().GoToUrl("https://www.ultimateqa.com/simple-html-elements-for-automation");

            Table = Driver.FindElement(By.Id("htmlTableId"));

            IList<IWebElement> htmlTableTr = Table.FindElements(By.XPath("//table[@id='htmlTableId']/tbody/tr"));
            var columnCounter = 1;

            foreach (var itemtr in htmlTableTr)
            {
                var tr = itemtr;
                IList<IWebElement> htmlTableTd = tr.FindElements(By.XPath("./*"));

                foreach (var itemtd in htmlTableTd)
                {
                    var td = itemtd;
                    if(td.Text == "Automation Testing Architect")
                    {
                         columnIndex = columnCounter;
                 
                    }

                    if(td.Text == "Salary")
                    {

                         rowIndex = htmlTableTd.IndexOf(itemtd) + 1;

                    }
                }
                columnCounter++;
            }

            var locator = string.Format("//table[@id='htmlTableId']/tbody/tr[{0}]/td[{1}]", rowIndex, columnIndex);
            var Salary = Table.FindElement(By.XPath(locator)).Text;
        }


       [TestMethod]

    public void Manipulation()

    {
            Driver.Navigate().GoToUrl("https://www.ultimateqa.com/filling-out-forms/");

            Driver.FindElement(By.Id("et_pb_contact_name_1")).SendKeys("Name");
            Driver.FindElement(By.Id("et_pb_contact_message_1")).SendKeys("Text Message");
            Driver.FindElement(By.CssSelector("button.et_pb_contact_submit:nth-child(1)")).Submit();

            

            var name = Driver.FindElements(By.Id("et_pb_contact_name_1"));
            name[0].SendKeys("Name");
            var message = Driver.FindElements(By.Id("et_pb_contact_message_1"));
            message[0].SendKeys("Message");
            var captcha = Driver.FindElement(By.ClassName("et_pb_contact_captcha_question"));

            var table = new DataTable();
            var answer = table.Compute(captcha.Text, "");

            Driver.FindElement(By.XPath("//input[@type='text'][@name='et_pb_contact_captcha_1']")).SendKeys(answer.ToString());

            var submitButton = Driver.FindElements(By.ClassName("et_contact_bottom_container"));

            submitButton[0].Submit();
            var successMessage = Driver.FindElements(By.ClassName("et-pb-contact-message"))[1].FindElement(By.TagName("p"));
            Assert.IsTrue(successMessage.Text.Equals("Success"));
        }

        [TestCleanup]

        public void TestCleanUp()
        {
            Driver.Close();
            Driver.Quit();

        }
}

}