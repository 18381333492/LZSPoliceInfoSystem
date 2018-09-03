using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class PagingRet
    {

        /// <summary>
        /// 当前页码数
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 最大条数
        /// </summary>
        public int total { get; set; } = 0;

        /// <summary>
        /// 分页的结果
        /// </summary>
        public object rows { get; set; }

        /// <summary>
        /// 其它数据
        /// </summary>
        public object data { get; set; }

        /// <summary>
        /// 转化为Json字符串
        /// </summary>
        /// <returns></returns>
        public string toJson()
        {
            return JsonHelper.toJson(this);
        }
    }
}