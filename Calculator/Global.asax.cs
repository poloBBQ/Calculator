using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

//Assembly attribute to watch the log4net configuration file for changes
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Calculator
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
