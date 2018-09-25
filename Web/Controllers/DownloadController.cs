using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using Common;

namespace Web.Controllers
{
    public class DownloadController : Controller
    {
        // GET: Download
        public ActionResult Index(int ID)
        {
            using (var db=new Entities())
            {
                var acticle=db.TG_Article.Find(ID);
                string fileName = acticle.sFileName;
                string filePath = AppDomain.CurrentDomain.BaseDirectory + "Files//" + fileName;
                FileStream fs = new FileStream(filePath, FileMode.Open);
                return File(fs, "application/ms-excel", acticle.sTitle);

            }
        }

        /// <summary>
        /// 更新阅读次数
        /// </summary>
        /// <param name="ID"></param>
        public ActionResult GetCount(int ID)
        {
            using (var db = new Entities())
            {
                var Article =db.TG_Article.Find(ID);
                var res =db.Database.ExecuteSqlCommand(string.Format("update TG_Article set iCount=iCount+1 where ID={0}", ID));
                var result = new Result();
                if (res > 0)
                {
                    result.data = Article.iCount + 1;
                    result.success = true;
                }
                return Content(result.toJson());
            }
        }


        /// <summary>
        /// 客户留言
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public ActionResult Insert(TG_Client client)
        {
            using (var db = new Entities())
            {
                var result = new Result();
                client.dInsertTime = DateTime.Now;
                db.TG_Client.Add(client);
                var res = db.SaveChanges();
                if (res > 0)
                {
                    result.info = "留言成功";
                    result.success = true;
                }
                return Json(result);
            }
        }

        /// <summary>
        /// 获取访问量
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRequestCount()
        {
            using (var db=new Entities())
            {
                var result = new Result();
                var count = db.TG_WebSite.FirstOrDefault().iCount;
                result.data = count;
                result.success = true;
                return Json(result);
            }
        }

        /// <summary>
        /// 获取图片宣传栏
        /// </summary>
        /// <returns></returns>
        public ActionResult GetImgList()
        {
            using (var db=new Entities())
            {
               var category=db.TG_Category.Find(42);
                var result = new Result();
                result.success = true;
                result.data = new
                {
                    img = category.sPictureUrl,
                    url = category.sKeyWord
                };
                return Json(result);
            }
        }

        /// <summary>
        /// 分页获取留言数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPageWords(PageInfo pageInfo, string sEmail)
        {
            using (var db = new Entities())
            {
                var query = db.TG_Client.Where(m=>m.sEmail== sEmail).OrderByDescending(m => m.dInsertTime).AsQueryable();
                var result = new Result();
                result.pageResult.page = pageInfo.page;
                result.pageResult.total = query.Count();
                query = query.Skip((pageInfo.page - 1) * pageInfo.rows).Take(pageInfo.rows);
                result.pageResult.rows = query;
                return Content(result.pageResult.toJson());
            }
        }

        /// <summary>
        /// 获取留言详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult GetWordsDetail(int ID)
        {
            using (var db = new Entities())
            {
                var result = new Result();
                var detail = db.TG_Client.Find(ID);
                result.data =JsonHelper.toJson(detail);
                result.success = true;
                return Json(result);
            }
        }
    }
}