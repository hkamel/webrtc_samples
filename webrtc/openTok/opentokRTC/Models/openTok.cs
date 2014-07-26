using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenTokApi.Core;
using System.Configuration;
namespace opentokRTC.Models
{
    public class openTok
    {

        public string ApiKey { get; set; }
        public string SessionId { get; set; }
        public string Token { get; set; }


        public openTok(string REMOTE_ADDR)
        {

            
            OpenTok ot = new OpenTok();

            Dictionary<string, object> options = new Dictionary<string, object>();
         
              options.Add(SessionProperties.P2PPreference, "enabled");  

            this.SessionId = ot.CreateSession(REMOTE_ADDR, options);
            this.Token = ot.GenerateToken(this.SessionId);
            this.ApiKey = ConfigurationManager.AppSettings["opentok.key"];
           
        
        
        }

    }
}