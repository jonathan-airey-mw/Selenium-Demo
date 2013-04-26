using System;
using OpenQA.Selenium;

namespace Smoking_Gun
{
    public class SourceConsole
    {
        private IWebElement _bonusElement;
        private IWebElement _approvalWeightElement;
        private IWebElement _sourceElement;
        private readonly IWebElement _row;
        public SourceConsole(IWebElement row)
        {
            _row = row;
            SetMembers();
        }
        private void SetMembers()
        {
            try
            {
            string sourceId = _row.FindElement(By.CssSelector("span")).GetAttribute("id");
            SetSourceElement(sourceId);
            SetBonusElement(sourceId);
            SetApprovalWeight(sourceId);
            }
            catch (Exception)
            {
  
            }
        }
        private void SetSourceElement(string sourceId)
        {
            _sourceElement = _row.FindElement(By.Id(sourceId));
        }
        private void SetBonusElement(string sourceId)
        {
            _bonusElement = _row.FindElement(By.Id(sourceId.Replace("lblSourceName", "lblBonus")));
        }
        private void SetApprovalWeight(string sourceId)
        {
            _approvalWeightElement = _row.FindElement(By.Id(sourceId.Replace("lblSourceName", "lblApprovalWeight")));
        }
        public bool IsInvalidRow()
        {
            return (_sourceElement != null && _bonusElement != null && _approvalWeightElement != null && _approvalWeightElement.Text != "1");
        }
        public string Source
        {
            get { return _sourceElement.Text; }
        }
        public string ApprovalWeight
        {
            get { return _approvalWeightElement.Text; }
        }
        public string Bonus
        {
            get { return _bonusElement.Text; }
        }
    }
}
