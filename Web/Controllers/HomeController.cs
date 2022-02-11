using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            int hour = DateTime.Now.Hour;
            ViewBag.Greeting = hour < 12 ? "Good Moning" : "Good Afternoon";
            return View();
        }
        [HttpGet]
        public IActionResult ContactForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FormContact(Contact contact)
        {
            using (ContactsContext db = new ContactsContext())
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
            }
            return View("ContactAdded", contact);
        }
        public IActionResult ContactsList(string group = null)
        {
            List<Contact> c = null;
            using (ContactsContext db = new ContactsContext())
            {
                if (group == null)
                    ViewBag.Contacts = db.Contacts.ToList();
                else if (group == "All")
                    c = db.Contacts.ToList();
                else
                    c = db.Contacts.Where((e) => e.Group == group).ToList();
            }
            if (group == null)
                return View();
            else
                return Json(c);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
