using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

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
    }
}