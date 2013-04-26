using System.Diagnostics;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;

namespace Smoking_Gun
{
    public class TestingClass
    {
        private IWebDriver _driver;
        private string _baseUrl;
        

        [SetUp]
        public void SetupTest()
        {
            _driver = new InternetExplorerDriver();
            _baseUrl = ConfigurationManager.AppSettings["baseURL"];
        }

        [TearDown]
        public void TeardownTest()
        {
                _driver.Quit();
        }

        [Test]
        public void BadTest()
        {
            string url = _baseUrl + ConfigurationManager.AppSettings["webPage"];
            _driver.Navigate().GoToUrl(_baseUrl + "/OpsCenter/SourceConsole.aspx");
            IWebElement table = _driver.FindElement(By.CssSelector("table.specialClass"));
            List<SourceConsole> listSourceConsole = table.FindElements(By.CssSelector("tr")).Select(row => new SourceConsole(row)).Where(sourceConsole => sourceConsole.IsInvalidRow()).ToList();
            PrintInvalidRows(listSourceConsole);
            Assert.AreEqual(0, listSourceConsole.Count);
        }

        [Test]
        public void BetterTest()
        {
            string url = _baseUrl + ConfigurationManager.AppSettings["webPage"];
            List<SourceConsole> listSourceConsole = new List<SourceConsole>();
            _driver.Navigate().GoToUrl(_baseUrl + "/OpsCenter/SourceConsole.aspx");
            IReadOnlyCollection<IWebElement> tables = _driver.FindElements(By.CssSelector("table"));
            foreach (ReadOnlyCollection<IWebElement> rows in from table in tables where table.GetAttribute("id").Contains("SourceGrid") select table.FindElements(By.CssSelector("tr")))
            {
                listSourceConsole.AddRange(rows.Select(row => new SourceConsole(row)).Where(sourceConsole => sourceConsole.IsInvalidRow()));
            }
            PrintInvalidRows(listSourceConsole);
            Assert.AreEqual(0, listSourceConsole.Count);
        }

        private void PrintInvalidRows(List<SourceConsole> listSourceConsole)
        {
            foreach (var sourceConsole in listSourceConsole)
            {
                Debug.WriteLine(string.Format("Source:{0} \t \t Weight:{1}", sourceConsole.Source, sourceConsole.ApprovalWeight));
            }
        }
    }
}