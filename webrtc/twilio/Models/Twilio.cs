using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Twilio;
using Twilio.TwiML;

namespace twilio.Models
{
    public class Twilio
    {
        string Twilio_FromNumber;
        string Twilio_AccountSid;
        string Twilio_AuthtTok;
        string WebURL;
        public Twilio()
        {
            Twilio_FromNumber = System.Configuration.ConfigurationManager.AppSettings["Twilio.FromNumber"].ToString();
            Twilio_AccountSid = System.Configuration.ConfigurationManager.AppSettings["Twilio.AccountSid"].ToString();
            Twilio_AuthtTok = System.Configuration.ConfigurationManager.AppSettings["Twilio.AuthTok"].ToString();
            WebURL = System.Configuration.ConfigurationManager.AppSettings["WebURL"].ToString();
           

        }

        public string SendSMS(string Message,  string phone )
        {
            string error = "";
            string FROM = Twilio_FromNumber;
            var client = new TwilioRestClient(Twilio_AccountSid, Twilio_AuthtTok);

            
             var result = client.SendMessage(FROM,phone, Message); 

           if  (result.RestException != null)
           { error = result.RestException.Message; }

           return error;
        }

        public string SendCall(string Message, string phone )
        {
            string error = "";
            string FROM = Twilio_FromNumber;
            var client = new TwilioRestClient(Twilio_AccountSid, Twilio_AuthtTok);
            CallOptions options = new CallOptions();

            options.From = FROM;
            options.To = phone;



            options.Url = WebURL + "twiliomessagesHandler.ashx?message=" + HttpContext.Current.Server.UrlEncode(Message) + "";

            // Place the call.
            var call = client.InitiateOutboundCall(options);

             if  (call.RestException != null )
             { error = call.RestException.Message; }

             return error;
        }

    }
}