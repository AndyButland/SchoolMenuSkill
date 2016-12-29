namespace SchoolMenuSkill.Models
{
    using System;
    using Newtonsoft.Json;
    using SchoolMenuSkill.Helpers;
    using SchoolMenuSkill.Providers;

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