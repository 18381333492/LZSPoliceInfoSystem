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

        public ActionResult NewIndex()
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
            var query = mangae.db.TG_Client.Where(m => m.sEmail == "0").OrderByDescending(m => m.ID).AsQueryable();
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

        /// <summary>
        /// 分页获取用户数据列表
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public ActionResult NewList(PageInfo pageInfo)
        {
            var query = mangae.db.TG_Client.Where(m=>m.sEmail=="1").OrderByDescending(m => m.ID).AsQueryable();
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

        /// <summary>
        /// 删除留言
        /// </summary>
        /// <param name="Ids"></param>
        public void Cancel(string Ids,string sign)
        {
            if (LoginStatus.iUserType == 0)
            {
                if (string.IsNullOrEmpty(LoginStatus.sCategoryIds) || LoginStatus.sCategoryIds.Contains(sign) == false)
                {
                    result.info = "您没有权限操作";
                }
                else
                {
                    var res = mangae.Delete<TG_Client>(Ids);
                    if (res)
                    {
                        result.success = true;
                    }
                }
            }
            else
            {
                var res = mangae.Delete<TG_Client>(Ids);
                if (res)
                {
                    result.success = true;
                }

            }
        }
                                    
        /// <summary>
        /// 回复
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Respone(int ID)
        {
            var model = mangae.db.TG_Client.Find(ID);
            return View(model);
        }

        /// <summary>
        /// 回复提交
        /// </summary>
        /// <param name="client"></param>
        public void ResponeCommit(TG_Client client,string sign)
        {
            if (LoginStatus.iUserType == 0)
            {
                if (string.IsNullOrEmpty(LoginStatus.sCategoryIds) || LoginStatus.sCategoryIds.Contains(sign) == false)
                {
                    result.info = "您没有权限操作";
                }
                else
                {
                    if (client.iState == 0)
                    {
                        client.iState = 1;
                        client.dDoneTime = DateTime.Now;
                    }
                    mangae.Edit<TG_Client>(client);
                    result.success = mangae.SaveChange();
                }
            }
            else
            {
                if (client.iState == 0)
                {
                    client.iState = 1;
                    client.dDoneTime = DateTime.Now;
                }
                mangae.Edit<TG_Client>(client);
                result.success = mangae.SaveChange();
            }
           
        }
    }
}