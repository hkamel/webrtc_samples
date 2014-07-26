using Microsoft.Owin;
using Owin;


[assembly: OwinStartup(typeof(opentokRTC.App_Start.Startup))]

namespace opentokRTC.App_Start
{
   
        public class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                app.MapSignalR();
            }
        }
     
}