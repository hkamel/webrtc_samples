using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Twilio.TwiML;
namespace twilio.Models
{
    public class twiliomessages: IHttpHandler
	{

        public void ProcessRequest(HttpContext context)
        {
            var twiml = new  TwilioResponse();
            if (context.Request["message"] != null)
            {
                var message = HttpContext.Current.Server.UrlDecode(context.Request["message"].ToString());

                twiml.Say(message);
            }

            else
                twiml.Say("Good Morning, I am ready");
            context.Response.Clear();
            context.Response.ContentType = "text/xml";
            context.Response.Write(twiml.ToString());
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

	}
}
