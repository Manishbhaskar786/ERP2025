using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using ERPack.Controllers;

namespace ERPack.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : ERPackControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
