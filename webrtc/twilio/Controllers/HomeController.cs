using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using twilio.Models;
namespace twilio.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string toPhoneNumber, string messsage, string Command)
        {
            twilio.Models.Twilio tw = new Models.Twilio();
             string error="";
         if (Command =="SMS")
             error = tw.SendSMS(messsage, toPhoneNumber);
            else
               error = tw.SendCall(messsage, toPhoneNumber);
          
          if (error != "")
              ViewBag.ErrorMEssage = error;
            return View();
        }


       


    }
}
