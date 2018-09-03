using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.App_Start;
using Web.Models;
using Common;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Web.Controllers
{
    /// <summary>
    /// 后台用户控制器
    /// </summary>
    public class UserController : BaseController
    {

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Edit(int ID)
        {
            ViewBag.ID = ID;
            return View();
        }

        /// <summary>
        /// 分页获取用户数据列表
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public ActionResult List(PageInfo pageInfo)
        {
            var query = mangae.db.TG_User.Where(m=>m.bIsSuper==false).OrderByDescending(m => m.ID).AsQueryable();
            if (!string.IsNullOrEmpty(pageInfo.sKeyWord))
            {//模糊查询
                query = query.Where(m => m.sUserName.Contains(pageInfo.sKeyWord) || m.sRealName.Contains(pageInfo.sKeyWord));
            }
            var total = query.Count();
            query = query.Skip((pageInfo.page - 1) * pageInfo.rows).Take(pageInfo.rows);

            result.pageResult.total = total;
            result.pageResult.rows = query;
            return Content(result.pageResult.toJson());
        }

        /// <summary>
        /// 分页获取登录日志列表
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public ActionResult LogList(PageInfo pageInfo)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                return View();
            }
            else
            {
                var query = mangae.db.TG_LoginLog.OrderByDescending(m =>m.dInsertTime).AsQueryable();
                if (!string.IsNullOrEmpty(pageInfo.sKeyWord))
                {//模糊查询
                    query = query.Where(m => m.sUserName.Contains(pageInfo.sKeyWord));
                }
                var total = query.Count();
                query = query.Skip((pageInfo.page - 1) * pageInfo.rows).Take(pageInfo.rows);
                result.pageResult.total = total;
                result.pageResult.rows = query;
                return Content(result.pageResult.toJson());
            }
        }

        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="user"></param>
        public void Insert(TG_User user)
        {
            if (mangae.db.TG_User.Any(m => m.sUserName == user.sUserName))
            {
                result.info = "该账户已存在";
            }
            else
            {
                user.sUserName = user.sUserName.Trim();
                user.iUserType = 0;
                user.bIsSuper = false;//默认非超级管理员
                user.sPassword = SecurityHelper.MD5(user.sPassword);
                mangae.Add<TG_User>(user);
                result.success = mangae.SaveChange();
            }
        }

        /// <summary>
        ///修改密码
        /// </summary>
        /// <param name="user"></param>
        public void Update(TG_User user)
        {
            var item = mangae.db.TG_User.Find(user.ID);
            item.sPassword = SecurityHelper.MD5(user.sPassword);
            result.success = mangae.SaveChange();
        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="Ids"></param>
        public void Cancel(string Ids)
        {
            result.success = mangae.Delete<TG_User>(Ids);
        }

        /// <summary>
        /// 生成图片验证码
        /// </summary>
        /// <returns></returns>
        [NoLogin]
        public FileResult MakePictureCode()
        {
            string sCode = PictureCodeHelper.CreateValidateCode(6);
            var code = PictureCodeHelper.CreateValidateGraphic(sCode);
            Session[sCodeSessionKey] = sCode;
            return File(code, "@image/jpeg");
        }


        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="sUserName"></param>
        /// <param name="sPassword"></param>
        /// <param name="sCode"></param>
        /// <returns></returns>
        [NoLogin]
        public ActionResult Login(string sUserName,string sPassword,string sCode)
        {
            if (!Request.IsAjaxRequest())
            {
                return View();
            }
            else
            {
                if (string.IsNullOrEmpty(sCode))
                {
                    result.info = "参数错误";
                    return Json(result);
                }
                if (sCode != Convert.ToString(Session[sCodeSessionKey]))
                {
                    result.info = "验证码错误";
                    return Json(result);
                }
                sPassword = SecurityHelper.MD5(sPassword);
                string Ip = Request.UserHostAddress;
                var user=mangae.db.TG_User.SingleOrDefault(m => m.sUserName == sUserName && m.sPassword == sPassword);
                if (user != null)
                {//登录成功
                    var now = DateTime.Now;
                    var log = mangae.db.TG_LoginLog.Where(m => m.sUserName == sUserName).
                        OrderByDescending(m => m.dInsertTime).FirstOrDefault();
                    LoginStatus = new LoginCacheInfo();
                    LoginStatus.ID = user.ID;
                    LoginStatus.sUserName = user.sUserName;
                    LoginStatus.bIsSuper = user.bIsSuper;
                    if (log != null)
                    {
                        LoginStatus.dLoginTime = log.dInsertTime;
                        LoginStatus.Ip = log.Ip;
                        LoginStatus.isFirst = false;
                    }
                    else
                    {
                        LoginStatus.dLoginTime = now;
                        LoginStatus.Ip = Ip;
                        LoginStatus.isFirst = true;
                    }
                    Session[sUserSessionKey] = LoginStatus;
                    result.success = true;
                    result.info = "登录成功";
                    Task.Factory.StartNew(() =>
                    {
                        InsertLog(sUserName, Ip, now);
                    });
                }
                else
                {//登录失败
                    result.info = "用户名或密码错误!";
                }
                Session[sCodeSessionKey] = null;
                return Json(result);
            }
        }


        /// <summary>
        /// 插入登录日志
        /// </summary>
        private void InsertLog(string sUserName,string Ip,DateTime now)
        {
            mangae.Add<TG_LoginLog>(new TG_LoginLog()
            {
                sUserName = sUserName,
                dInsertTime = now,
                Ip= Ip
            });
           var res=mangae.SaveChange();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult AlterPwd(string sOldPwd, string sNewPwd)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                return View();
            }
            else
            {
                if (string.IsNullOrEmpty(sOldPwd) || string.IsNullOrEmpty(sNewPwd))
                {
                    result.info = "参数错误";
                    return Json(result);
                }
                //检查原密码是否正确
                sOldPwd= SecurityHelper.MD5(sOldPwd);
                sNewPwd = SecurityHelper.MD5(sNewPwd);
                if(mangae.db.TG_User.Any(m=>m.sUserName==LoginStatus.sUserName&&m.sPassword== sOldPwd))
                {
                    List<SqlParameter> SqlParsArray = new List<SqlParameter>();
                    SqlParsArray.Add(new SqlParameter("ID", LoginStatus.ID));
                    SqlParsArray.Add(new SqlParameter("sPassword", sNewPwd));
                    var res =mangae.ExcuteBySql("update TG_User set sPassword=@sPassword where ID=@ID", SqlParsArray.ToArray());
                    if (res > 0)
                    {
                        result.success = true;
                    }
                    else
                        result.info = "密码修改失败";
                }
                else
                {
                    result.info = "原密码输入错误";
                    return Json(result);
                }
                return Json(result);
            }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Quit()
        {
            //清空登录信息
            Session.Remove(sUserSessionKey);
            result.success = true;
            return Json(result);
        }
    }
}