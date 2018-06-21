using AspNetCoreWebMefDI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Composition;

namespace AspNetCoreWebMefDI.Controllers
{
    public class HomeController : Controller
    {
        [Import]
        private IValueProvider ValueProvider { get; set; }
        
        public IActionResult Index()
        {
            return View("Index", this.ValueProvider.GetValue("sayHello"));
        }
    }
}