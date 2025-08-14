using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace ServiceYouNow.src.automation
{
    public class PSGDatabaseAccessRequest
    {
        private readonly IPage _page;
        private readonly string _environment;
        private readonly string _serverName;
        private readonly string _accessLevel;
        private readonly string _workItemNumber;
        private readonly string _businessCase;

        public PSGDatabaseAccessRequest
        (
            IPage page,
            string environment,
            string serverName,
            string accessLevel,
            string workItemNumber,
            string businessCase)
        {
            _page = page;
            _environment = environment;
            _serverName = serverName;
            _accessLevel = accessLevel;
            _workItemNumber = workItemNumber;
            _businessCase = businessCase;

        }

        public async Task FillFormAsync()
        {
            await Locators.Searchbar(_page).ClickAsync();
            await Locators.Searchbar(_page).FillAsync("PSG Database Access Request");
            await Locators.SearchButton(_page).ClickAsync();
            await Locators.PSGDatabaseAccessRequest(_page).ClickAsync();
            
            await Locators.ActionDropdown(_page).ClickAsync();
            await Locators.ActionDropdownOption_AddTemporary(_page).ClickAsync();
            
            await Locators.EnvironmentDropdown(_page).ClickAsync();
            await Locators.EnvironmentDropdownOption(_page, _environment).ClickAsync();
            
            if (_environment == "PRD" || 
                _environment == "EBF" || 
                _environment == "SPR" ||
                _environment == "SAT")
            {
                await Locators.EnvironmentServerDropdown(_page).ClickAsync();
                await Locators.EnvironmentServerDropdownOption(_page, _serverName).ClickAsync();
            }
            
            string accessStartDate = DateTime.Today.ToString("dd-MMM-yy");
            var accessTime = 30;
            if(_environment == "PRD" || 
               _environment == "EBF" ||
               _environment == "SPR")
            {
                accessTime = 7;
            }
            string accessEndDate = DateTime.Today.AddDays(accessTime).ToString("dd-MMM-yy");
            await Locators.AccessStartDate(_page).ClickAsync();
            await Locators.AccessStartDate(_page).FillAsync(accessStartDate);
            
            await Locators.AccessEndDate(_page).ClickAsync();
            await Locators.AccessEndDate(_page).FillAsync(accessEndDate);
       
            await Locators.AccessDropdown(_page).ClickAsync();
            await Locators.AccessDropdownOption(_page, _accessLevel).ClickAsync();

            await Locators.AssociatedWorkItemNumber(_page).ClickAsync();
            await Locators.AssociatedWorkItemNumber(_page).FillAsync(_workItemNumber);

            await Locators.BusinessCase(_page).ClickAsync();
            await Locators.BusinessCase(_page).FillAsync(_businessCase);
            await Locators.HomeButton(_page).ClickAsync();
        }
    }
}