using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    /// <summary>
    /// 用户登录的缓存信息
    /// </summary>
    [Serializable]
    public class LoginCacheInfo
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID
        {
            get;set;
        }

        /// <summary>
        /// 账号
        /// </summary>
        public string sUserName
        {
            get;set;
        }

        /// <summary>
        /// 是否是超级管理员
        /// </summary>
        public bool bIsSuper
        {
            get; set;
        }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime dLoginTime
        {
            get;set;
        }

        /// <summary>
        /// 上次登录IP地址
        /// </summary>
        public string Ip
        {
            get;set;
        }

        public bool isFirst
        {
            get;set;
        }
    }
}