using Microsoft.AspNet.SignalR;
using opentokRTC.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace opentokRTC.Controllers
{
    public class RTCHub: Hub 
    {

        public static ConcurrentDictionary<string, User> Users = new ConcurrentDictionary<string, User>();
        public static connections connections = new connections();


        public User GetConnected(string username, string Remote_Address)
        {
             User user;
             connections.Add(Context.ConnectionId);
             var ot=new openTok (Remote_Address);
             user = new User(username,Context.ConnectionId, ot);
                      
            Users.TryAdd(Context.ConnectionId, user);
            Clients.Clients(connections.AllExcept(Context.ConnectionId)).getNewOnlineUser(user);
            Clients.Client(Context.ConnectionId).getOnlineUsers(Users);


            return user;
        }


        public void BeginCall(string toConnectionId)
        {
            
            var touser = Users[toConnectionId];
            var caller = Users[Context.ConnectionId];
            List<User> users = new List<User>();
            
            Clients.Client(toConnectionId).notifybeginCall(touser, caller);


        }

        public void CallRejectedSignal(string CallerConnectionId)
        {
            
            var self = Users[Context.ConnectionId];
                  
            Clients.Client(CallerConnectionId).notifyCallrejected("Call rejected by : " + self.Name);
        }

        public void CallAcceptedSignal(string CallerConnectionId)
        {
            
             


            var caller = Users[CallerConnectionId];
            var self = Users[Context.ConnectionId];
             Clients.Client(CallerConnectionId).notifyCallaccepted(caller, self);
            
        }

        public void EndCall(string CallerConnectionId)
        {
            try
            {
               

                User caller = new User();
                User self = new User();
                if (Users.ContainsKey(Context.ConnectionId))
                {
                   self = Users[Context.ConnectionId];
                   caller = Users[CallerConnectionId];
                  Clients.Client(CallerConnectionId).notifyCallend(caller, self); 
                    
                }
            }
            catch (Exception exp)
            {   }
        }

        private void GetDisConnected(string ConnectionId)
        {
            if (Users.ContainsKey(ConnectionId) == true)
            {
                User user;
                User currentuser = Users[ConnectionId];
                
                Users.TryRemove(ConnectionId.ToString(), out user);
                connections.Remove(ConnectionId);
                Clients.Clients(connections.AllExcept(ConnectionId)).disconnected(user);
            }
        }

        public void GetDisConnected()
        {GetDisConnected(Context.ConnectionId);}

        public override Task OnDisconnected()
        {
            GetDisConnected(Context.ConnectionId);

            return base.OnDisconnected();
        }

    }
}