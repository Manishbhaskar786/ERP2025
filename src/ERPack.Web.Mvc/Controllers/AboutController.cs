using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using ERPack.Controllers;

namespace ERPack.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : ERPackControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
