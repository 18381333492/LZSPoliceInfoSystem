using Common;
using RazorBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraceLogs;
using Web.Models;
using Web.Server;

namespace Web.App_Start
{
    /// <summary>
    /// 基础Controller
    /// </summary>
    public class BaseController: Controller
    {

        /// <summary>
        /// 日志组件
        /// </summary>
        private static ILogger logger = LoggerManager.Instance.GetSLogger("Web");
        protected static readonly string sUserSessionKey = "@UserInfo";
        protected static readonly string sCodeSessionKey = "@CodeInfo";


        /// <summary>
        /// EF的封装
        /// </summary>
        protected EFHelper mangae;

        /// <summary>
        /// 返回结果
        /// </summary>
        protected Result result;

        /// <summary>
        /// 管理员的登录信息
        /// </summary>
        public LoginCacheInfo LoginStatus;


        /// <summary>
        /// 初始化构造函数
        /// </summary>
        public BaseController()
        {
            mangae = new EFHelper();
            result = new Result();
        }

        /// <summary>
        /// 在Action之前调用
        /// tip:主要来验证用户登录
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //是否需要验证登录
            bool needLogin = filterContext.ActionDescriptor.GetCustomAttributes(typeof(NoLogin), true).Length == 1 ? false : true;
            if (needLogin)
            { //验证登录
                bool IsAjax = filterContext.HttpContext.Request.IsAjaxRequest();
                if (Session[sUserSessionKey] != null)
                {
                    LoginStatus = (LoginCacheInfo)Session[sUserSessionKey];
                }
                else
                {
                    if(IsAjax&& filterContext.HttpContext.Request.HttpMethod.ToUpper() == "POST")
                    {
                        result.over = true;
                        filterContext.Result = Json(result);
                    }
                    else
                    {
                        //登录失效跳转登录
                        filterContext.Result = Redirect("/User/Login");
                    }   
                }
            }
        }


        /// <summary>
        /// 在Action之后调用
        /// 主要处理返回的数据
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string sActionName = filterContext.ActionDescriptor.ActionName;//获取action的名字
            if (sActionName.ToLower() == "login")
            {
                sActionName = sActionName.Substring(0, 1).ToUpper() + sActionName.Substring(1, sActionName.Length - 1).ToLower();
            }
            var request = filterContext.HttpContext.Request;
            var actionMethod = filterContext.Controller
              .GetType()
              .GetMethod(sActionName);//获取访问方法   
            if (actionMethod.ReturnType.Name.ToString() == "Void")
            {//POST的返回结果处理
                filterContext.Result = Content(result.toJson()); /**统一处理ajax的返回结果**/
            }
        }

        /// <summary>
        /// 捕捉全局异常
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            logger.Info("请求链接:" + filterContext.HttpContext.Request.Url.AbsolutePath);
            logger.Info("WebController异常:" + filterContext.Exception.Message);
            logger.Fatal("WebController异常:" + filterContext.Exception);
            result.success = false;
            result.info = "系统错误!";
            filterContext.Result = Content(JsonHelper.toJson(result));
        }
    }
}