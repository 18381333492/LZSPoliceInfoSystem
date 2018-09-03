using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.App_Start
{
    /// <summary>
    /// 使用[NoLogin]标注时候不需要验证登录
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
    public class NoLogin : Attribute
    {
    }
}