using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace opentokRTC.Models
{
    public class connections
    {
        public static List<string> ConnectionsList = new List<string>();

        public void Add(string ConnectionId)
        {
            ConnectionsList.Add(ConnectionId);
        }
        public void Remove(string ConnectionId)
        {
            ConnectionsList.Remove(ConnectionId);
        }


        public List<string> AllExcept(params string[] ConnectionIds)
        {
            var connections = ConnectionsList.Except(ConnectionIds).ToList();
            return connections;
        }


    }
}