namespace ServiceYouNow.Helpers
{
    public class SprintHelper
    {
        public static SprintInfo GetSprintInfo(DateTime currentDate)
        {
            DateTime sprintStart = new DateTime(2025, 8, 25); // Sprint 295 start
            int sprintNumber = 295;

            // Move forward until we find the sprint that contains currentDate
            while (true)
            {
                List<DateTime> sprintDays = new List<DateTime>();
                DateTime tempDate = sprintStart;

                // Generate 10 working days (skip weekends)
                while (sprintDays.Count < 10)
                {
                    if (tempDate.DayOfWeek != DayOfWeek.Saturday && tempDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        sprintDays.Add(tempDate);
                    }
                    tempDate = tempDate.AddDays(1);
                }

                if (currentDate.Date >= sprintDays.First() && currentDate.Date <= sprintDays.Last())
                {
                    int dayNumber = sprintDays.IndexOf(currentDate.Date) + 1;
                    return new SprintInfo
                    {
                        SprintNumber = sprintNumber,
                        DayNumber = dayNumber,
                        SprintDates = sprintDays
                    };
                }

                // Move to next sprint
                sprintStart = tempDate; // tempDate is already the next Monday
                sprintNumber++;
            }
        }
    }
}
