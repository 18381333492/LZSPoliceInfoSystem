using RazorBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web.Server;
using Web.Models;
using TraceLogs;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static ILogger logger = LoggerManager.Instance.GetSLogger("Web");
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //获取访问量
        }

        ///// <summary>
        ///// 开始回话
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void Session_Start(object sender, EventArgs e)        //{
        //    //不是每次请求都调用
        //    //会话开始时执行
        //}

        void Application_BeginRequest(object sender, EventArgs e)
        {      
            //每次请求时第一个出发的事件，这个方法第一个执行
            try
            {
                using (var db = new Entities())
                {
                    var res=db.Database.ExecuteSqlCommand("update TG_WebSite set iCount=iCount+1");
                    logger.Info("更新访问返回结果:"+res.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.Info("更新访问量失败");
                logger.Fatal(ex.Message);
                throw;
            }       
        }

    }
}
