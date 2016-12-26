namespace SchoolMenuSkill.Controllers
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using SchoolMenuSkill.Speechlet;

    public class HomeController : ApiController
    {
        [Route("api/test")]
        [HttpGet]
        public string Test()
        {
            return "Hello";
        }

        [Route("api/menu")]
        [HttpPost]
        public async Task<HttpResponseMessage> Menu()
        {
            var speechlet = new MenuSpeechlet();
            return await speechlet.GetResponseAsync(Request);
        }
    }
}
