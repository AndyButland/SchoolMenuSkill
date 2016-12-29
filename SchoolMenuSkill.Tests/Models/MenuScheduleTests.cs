namespace SchoolMenuSkill.Tests.Models
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SchoolMenuSkill.Models;
    using SchoolMenuSkill.Providers;

    [TestClass]
    public class MenuScheduleTests
    {
        [TestMethod]
        public void MenuSchedule_GetsMenuForToday()
        {
            // Arrange
            var schedule = CreateSchedule();

            // Act
            var date = GetTestDate();
            var menu = schedule.GetMenuForDate(date);

            // Assert
            Assert.AreEqual(
                "On Monday 5th December, the menu is Pasta with tomato sauce, then Fish with Spinach, finishing with Ice cream",
                menu.ToString(date, GetDateTimeProvider()));
        }

        [TestMethod]
        public void MenuSchedule_GetsMenuForNextDay()
        {
            // Arrange
            var schedule = CreateSchedule();

            // Act
            var date = GetTestDate().AddDays(1);
            var menu = schedule.GetMenuForDate(date);

            // Assert
            Assert.AreEqual(
                "On Tuesday 6th December, the menu will be Rice, then Chicken with Peas, finishing with Mousse",
                menu.ToString(date, GetDateTimeProvider()));
        }

        [TestMethod]
        public void MenuSchedule_GetsMenuForDayInFirstWeek()
        {
            // Arrange
            var schedule = CreateSchedule();

            // Act
            var date = GetTestDate().AddDays(2);
            var menu = schedule.GetMenuForDate(date);

            // Assert
            Assert.AreEqual(
                "On Wednesday 7th December, the menu will be Spaghetti, then Beef with Carrots, finishing with Cake",
                menu.ToString(date, GetDateTimeProvider()));
        }

        [TestMethod]
        public void MenuSchedule_GetsMenuForDayInSecondWeek()
        {
            // Arrange
            var schedule = CreateSchedule();

            // Act
            var date = GetTestDate().AddDays(7).AddDays(2);
            var menu = schedule.GetMenuForDate(date);

            // Assert
            Assert.AreEqual(
                "On Wednesday 14th December, the menu will be Spaghetti, then Beef with Carrots, finishing with Cake",
                menu.ToString(date, GetDateTimeProvider()));
        }

        [TestMethod]
        public void MenuSchedule_GetsMenuForDayInThirdWeek()
        {
            // Arrange
            var schedule = CreateSchedule();

            // Act
            var date = GetTestDate().AddDays(14).AddDays(2);
            var menu = schedule.GetMenuForDate(date);

            // Assert
            Assert.AreEqual(
                "On Wednesday 21st December, the menu will be Spaghetti, then Beef with Carrots, finishing with Cake",
                menu.ToString(date, GetDateTimeProvider()));
        }

        [TestMethod]
        public void MenuSchedule_ReturnNullForDatePriorToInitialMenuDate()
        {
            // Arrange
            var schedule = CreateSchedule();

            // Act
            var date = GetTestDate().AddDays(-5);
            var menu = schedule.GetMenuForDate(date);

            // Assert
            Assert.IsNull(menu);
        }

        private static MenuSchedule CreateSchedule()
        {
            return new MenuSchedule
            {
                InitialDate = GetTestDate(),
                Menu = new List<Menu>
                {
                    new Menu
                    {
                        WeekNumber = 1,
                        Day = "Monday",
                        Primo = "Pasta with tomato sauce",
                        Secondo = "Fish",
                        Contorno = "Spinach",
                        Dolce = "Ice cream"
                    },
                    new Menu
                    {
                        WeekNumber = 1,
                        Day = "Tuesday",
                        Primo = "Rice",
                        Secondo = "Chicken",
                        Contorno = "Peas",
                        Dolce = "Mousse"
                    },
                    new Menu
                    {
                        WeekNumber = 1,
                        Day = "Wednesday",
                        Primo = "Spaghetti",
                        Secondo = "Beef",
                        Contorno = "Carrots",
                        Dolce = "Cake"
                    },
                    new Menu
                    {
                        WeekNumber = 1,
                        Day = "Thursday",
                        Primo = "Pasta with ragu",
                        Secondo = "Pork",
                        Contorno = "Mushrooms",
                        Dolce = "Creme caramel"
                    },
                    new Menu
                    {
                        WeekNumber = 1,
                        Day = "Friday",
                        Primo = "Risotto",
                        Secondo = "Nut roast",
                        Contorno = "Sweetcorn",
                        Dolce = "Fruit"
                    }
                }
            };
        }

        private static DateTime GetTestDate()
        {
            return new DateTime(2016, 12, 5);
        }

        private IDateTimeProvider GetDateTimeProvider()
        {
            return new StubDateTimeProvider();
        }

        private class StubDateTimeProvider : IDateTimeProvider
        {
            public DateTime Now()
            {
                return GetTestDate();
            }
        }
    }
}
