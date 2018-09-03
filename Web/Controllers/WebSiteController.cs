using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.App_Start;
using Web.Models;

namespace Web.Controllers
{

    /// <summary>
    /// 站点设置控制器
    /// </summary>
    public class WebSiteController : BaseController
    {
        // GET: WebSite

        /// <summary>
        /// 站点设置
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var web = mangae.db.TG_WebSite.FirstOrDefault();
            if (web == null)
                web = new TG_WebSite();
            ViewBag.bIsSuper =LoginStatus.bIsSuper;
            return View(web);
        }

        /// <summary>
        /// 站点设置
        /// </summary>
        /// <param name="webSite"></param>
        public void Set(TG_WebSite webSite)
        {
            if (webSite.ID == 0)
            {//添加
                mangae.Add<TG_WebSite>(webSite);
                result.success = mangae.SaveChange();
            }
            else
            {//编辑
                mangae.Edit<TG_WebSite>(webSite);
                result.success = mangae.SaveChange();
            }
        }
    }
}