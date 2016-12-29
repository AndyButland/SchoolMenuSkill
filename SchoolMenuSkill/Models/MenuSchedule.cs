namespace SchoolMenuSkill.Models
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;
    using SchoolMenuSkill.Helpers;
    using SchoolMenuSkill.Providers;

    public class MenuSchedule
    {
        [JsonProperty("initialDate")]
        public DateTime InitialDate { get; set; }

        [JsonProperty("menu")]
        public List<Menu> Menu { get; set; }

        public Menu GetMenuForDate(DateTime date)
        {
            var daysBetween = (date - InitialDate).TotalDays;
            if (daysBetween < 0)
            {
                return null;
            }

            // Count forward to find index of menu for the requested date
            var dayOfWeek = DayOfWeek.Monday;
            var menuIndex = 0;
            for (var i = 0; i < daysBetween; i++)
            {
                dayOfWeek = IncrementDayOfWeek(dayOfWeek);

                // If a week-day, advance to next menu item
                if (IsWeekDay(dayOfWeek))
                {
                    menuIndex++;

                    // Reset to first item in menu once we've got to the end
                    if (menuIndex == Menu.Count)
                    {
                        menuIndex = 0;
                    }
                }
            }

            return Menu[menuIndex];
        }

        private static DayOfWeek IncrementDayOfWeek(DayOfWeek dayOfWeek)
        {
            if (dayOfWeek == DayOfWeek.Saturday)
            {
                return DayOfWeek.Sunday;
            }

            return (DayOfWeek)((int)dayOfWeek + 1);
        }

        private static bool IsWeekDay(DayOfWeek dayOfWeek)
        {
            return !(dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday);
        }
    }
}