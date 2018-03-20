using Glass.Mapper.Sc.Web.Mvc;
using SmartSitecore.Project.InlineForms.TestWebsite.Models;
using System.Web.Mvc;

namespace SmartSitecore.Project.InlineForms.TestWebsite.Controllers
{
    public class ComponentController : GlassController
    {
        [HttpGet]
        public ActionResult UseGlass()
        {
            return View("~/Views/InlineFormsTestWebsite/GlassComponentView.cshtml", GetContextItem<BasePage>());
        }

        [HttpGet]
        public ActionResult UseSitecore()
        {
            return View("~/Views/InlineFormsTestWebsite/ScComponentView.cshtml");
        }
    }
}