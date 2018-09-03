using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.App_Start;
using Web.Models;
using Common;

namespace Web.Controllers
{
    /// <summary>
    /// 客户控制器
    /// </summary>
    public class ClientController : BaseController
    {

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 添加用户咨询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(TG_Client client)
        {
            client.dInsertTime = DateTime.Now;
            mangae.Add<TG_Client>(client);
            result.success=mangae.SaveChange();
            return Json(result);
        }

        /// <summary>
        /// 分页获取用户数据列表
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public ActionResult List(PageInfo pageInfo)
        {
            var query = mangae.db.TG_Client.OrderByDescending(m => m.ID).AsQueryable();
            if (!string.IsNullOrEmpty(pageInfo.sKeyWord))
            {//模糊查询
                query = query.Where(m => m.sName.Contains(pageInfo.sKeyWord) || m.sPhone.Contains(pageInfo.sKeyWord));
            }
            var total = query.Count();
            query = query.Skip((pageInfo.page - 1) * pageInfo.rows).Take(pageInfo.rows);
            result.pageResult.total = total;
            result.pageResult.rows = query;
            return Content(result.pageResult.toJson());
        }
    }
}