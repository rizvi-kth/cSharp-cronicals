using System.Web.Http;

namespace OWIN_Web_API_Starter_Template1.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet]
        [Route("")]
        public string Index()
        {
            return "Web API Started successfully";
        }
    }
}