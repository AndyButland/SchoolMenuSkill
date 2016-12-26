namespace SchoolMenuSkill.Speechlet
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web;
    using AlexaSkillsKit.Slu;
    using AlexaSkillsKit.Speechlet;
    using AlexaSkillsKit.UI;
    using Newtonsoft.Json;
    using SchoolMenuSkill.Models;

    public class MenuSpeechlet : SpeechletAsync
    {
        private static MenuSchedule _menuSchedule;

        private const string DateKey = "Date";
        private const string MenuForDateIntent = "MenuForDateIntent";

        public override Task<SpeechletResponse> OnLaunchAsync(LaunchRequest launchRequest, Session session)
        {
            return Task.FromResult(GetWelcomeResponse());
        }

        public async override Task<SpeechletResponse> OnIntentAsync(IntentRequest intentRequest, Session session)
        {
            // Get intent from the request object.
            var intent = intentRequest.Intent;
            var intentName = intent?.Name;

            // Note: If the session is started with an intent, no welcome message will be rendered;
            // rather, the intent specific response will be returned.
            if (MenuForDateIntent.Equals(intentName))
            {
                return await GetMenuResponse(intent, session);
            }

            throw new SpeechletException("Invalid Intent");
        }

        public override Task OnSessionStartedAsync(SessionStartedRequest sessionStartedRequest, Session session)
        {
            return Task.FromResult(0);
        }

        public override Task OnSessionEndedAsync(SessionEndedRequest sessionEndedRequest, Session session)
        {
            return Task.FromResult(0);
        }

        private async Task<SpeechletResponse> GetMenuResponse(Intent intent, Session session)
        {
            // Retrieve date from the intent slot
            var dateSlot = intent.Slots[DateKey];

            // Create response
            string output;
            DateTime date;
            if (dateSlot != null && DateTime.TryParse(dateSlot.Value, out date))
            {
                // Retrieve and return the menu response
                if (_menuSchedule == null)
                {
                    _menuSchedule = await LoadMenuSchedule();
                }

                output = _menuSchedule.GetMenuForDate(date).ToString(date);
            }
            else
            {
                // Render an error since we don't know what the date requested is
                output = "I'm not sure which date you require, please try again.";
            }

            // Here we are setting shouldEndSession to false to not end the session and
            // prompt the user for input
            return BuildSpeechletResponse(intent.Name, output, false);
        }

        private SpeechletResponse GetWelcomeResponse()
        {
            var output = "Welcome to the English International School menu app. Please request the menu for a given date.";
            return BuildSpeechletResponse("Welcome", output, false);
        }

        private async Task<MenuSchedule> LoadMenuSchedule()
        {
            var filePath = HttpContext.Current.Server.MapPath("/App_Data/menu.json");
            using (var reader = new StreamReader(filePath))
            {
                var json = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<MenuSchedule>(json);
            }
        }

        /// <summary>
        /// Creates and returns the visual and spoken response with shouldEndSession flag
        /// </summary>
        /// <param name="title">Title for the companion application home card</param>
        /// <param name="output">Output content for speech and companion application home card</param>
        /// <param name="shouldEndSession">Should the session be closed</param>
        /// <returns>SpeechletResponse spoken and visual response for the given input</returns>
        private SpeechletResponse BuildSpeechletResponse(string title, string output, bool shouldEndSession)
        {
            // Create the Simple card content
            var card = new SimpleCard
            {
                Title = $"SessionSpeechlet - {title}",
                Content = $"SessionSpeechlet - {output}"
            };

            // Create the plain text output
            var speech = new PlainTextOutputSpeech { Text = output };

            // Create the speechlet response
            var response = new SpeechletResponse
            {
                ShouldEndSession = shouldEndSession,
                OutputSpeech = speech,
                Card = card
            };
            return response;
        }
    }
}