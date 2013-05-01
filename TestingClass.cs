using System.Diagnostics;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;

namespace Smoking_Gun {
    public class TestingClass {
        private IWebDriver _driver;
        private string _baseUrl;

        [SetUp]
        public void SetupTest() {
            _driver = new InternetExplorerDriver();
            _baseUrl = ConfigurationManager.AppSettings["baseURL"];
        }

        [TearDown]
        public void TeardownTest() {
            _driver.Quit();
        }

        [Test]
        public void BadTest() {
            string url = _baseUrl + "/OpsCenter/SourceConsole.aspx";

            _driver.Navigate().GoToUrl(url);

            var elements = _driver.FindElements(By.CssSelector("table.specialClass tr"));

            var invalidRows = ( from webElement in elements
                                let row = new SourceConsole(webElement)
                                where row.IsInvalidRow() == true
                                select row ).ToList();

            printRows(invalidRows);

            Assert.AreEqual(0, invalidRows.Count());
        }

        [Test]
        public void BetterTest() {
            string url = _baseUrl + "/OpsCenter/SourceConsole.aspx";
            
            _driver.Navigate().GoToUrl(url);

            var webElements = _driver.FindElements(By.CssSelector("table[id$='SourceGrid'] tr"));

            var invalidRows = ( from element in webElements
                                let row = new SourceConsole(element)
                                where row.IsInvalidRow()
                                select row ).ToList();

            printRows(invalidRows);
            Assert.AreEqual(1, invalidRows.Count);
        }

        private void printRows(IEnumerable<SourceConsole> sourceControlList) {
            foreach ( var sourceConsole in sourceControlList ) {
                Debug.WriteLine(string.Format("Source:{0} \t \t Weight:{1}", sourceConsole.Source, sourceConsole.ApprovalWeight));
            }
        }
    }
}