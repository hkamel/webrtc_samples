using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace opentokRTC.Models
{
    public class User
    {
        public string ConnectionId { get; set; }
        public string Name { get; set; }
        public openTok Opentok { get; set; }
        public User()
        { this.Name = ""; }
        public User(string name, string connectionId, openTok opentok)
        {
            this.Name = name;
           
            this.ConnectionId = connectionId;
            this.Opentok = opentok;
            
        }
    }

}