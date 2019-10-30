using Emrys.FlashLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //配置log4
            //log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/log4net.config")));

            //FlashLogger.Instance().Register();
            //Controllers.ValuesController aa = new Controllers.ValuesController();
            //var ret = aa.CsWebApiLog();
        }
    }
}
