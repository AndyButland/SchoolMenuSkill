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
            var dayOfWeek = DayOfWeek.Monday;
            var menuIndex = 0;
            if (daysBetween >= 0)
            {
                // For future date, count forward to find index of menu for the requested date
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
            }
            else
            {
                // For previous date, count backward to find index of menu for the requested date
                for (var i = 0; i >= daysBetween; i--)
                {
                    dayOfWeek = DecrementDayOfWeek(dayOfWeek);

                    // If a week-day, go back to previous menu item
                    if (IsWeekDay(dayOfWeek))
                    {
                        menuIndex--;
                        
                        // Reset to last item in menu once we've got to the beginning
                        if (menuIndex <= 0)
                        {
                            menuIndex = Menu.Count;
                        }
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

        private static DayOfWeek DecrementDayOfWeek(DayOfWeek dayOfWeek)
        {
            if (dayOfWeek == DayOfWeek.Sunday)
            {
                return DayOfWeek.Saturday;
            }

            return (DayOfWeek)((int)dayOfWeek - 1);
        }

        private static bool IsWeekDay(DayOfWeek dayOfWeek)
        {
            return !(dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday);
        }
    }

    public class Menu
    {
        [JsonProperty("week")]
        public int WeekNumber { get; set; }

        [JsonProperty("day")]
        public string Day { get; set; }

        [JsonProperty("primo")]
        public string Primo { get; set; }

        [JsonProperty("secondo")]
        public string Secondo { get; set; }

        [JsonProperty("contorno")]
        public string Contorno { get; set; }

        [JsonProperty("dolce")]
        public string Dolce { get; set; }

        public string ToString(DateTime date, IDateTimeProvider dateTimeProvider = null)
        {
            dateTimeProvider = dateTimeProvider ?? new DateTimeProvider();

            string verbTense;
            if (date.Date == dateTimeProvider.Now().Date)
            {
                verbTense = "is";
            }
            else if (date.Date < dateTimeProvider.Now().Date)
            {
                verbTense = "was";
            }
            else
            {
                verbTense = "will be";
            }

            return $"On {Day} {date.ToStringWithSuffix("d MMMM")}, the menu {verbTense} {Primo}, then {Secondo} with {Contorno}, finishing with {Dolce}";
        }
    }
}