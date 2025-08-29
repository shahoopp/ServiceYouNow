ServiceYouNow Automation
This project automates ServiceNow workflows using the Microsoft.Playwright library in C#. It is designed to streamline incident management and standard change processes by interacting with the ServiceNow UI programmatically.

🚀 Features
Automates login and navigation through ServiceNow.
Applies filters and exports incident data based on selected sprint dates.
Uploads exported files to relevant ServiceNow sections.
Fills out forms and schedules using date-specific information.
Supports concurrent automation across multiple tabs.
🛠️ Setup Instructions
Prerequisites
https://dotnet.microsoft.com/download
https://nodejs.org/ (required for Playwright)
PowerShell (for running Playwright installation script)
Install Playwright for .NET
Run the following command in PowerShell:


Replace netX.Y with your target framework (e.g., net6.0).

🧪 Usage
Configure Preferences
Before running the automation, ensure the following fields are set via the UI or PreferencesStorage:

Username
Password
WorkItemNumber
FullName
Notes
SelectedSprintDates (List of DateTime)
Launch Automation

Call the LaunchBrowserAsync() method. It will:

Validate preferences
Log into ServiceNow
Open one tab per selected date
Run automation for each tab sequentially
Automation Flow

Navigate to the ServiceNow homepage
Apply saved filters and select the appropriate date
Export incident data to Excel
Upload the file to the Standard Change section
Fill out deployment and scheduling fields
Leave tabs open for manual review
📁 File Naming Convention
Exported files are saved as:

Incidents_<MMM_dd_yyyy>.xlsx
Example: Incidents_Aug_25_2025.xlsx

📌 Notes
The automation expects dates in dd-MMM-yy format (e.g., 25-Aug-25).
You need to have a filter saved on the Incidents page labelled as "AUTOMATION_FILTER".
Tabs remain open after execution for manual inspection.
The automation is designed to be non-blocking and resilient to UI delays.