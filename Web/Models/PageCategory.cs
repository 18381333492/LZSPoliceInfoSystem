using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    /// <summary>
    /// 分页栏目
    /// </summary>
    public class PageCategory
    {
        /// <summary>
        /// 栏目
        /// </summary>
        public TG_Category Category
        {
            get;set;
        }

        /// <summary>
        /// 第几页
        /// </summary>
        public int Page
        {
            get;set;
        }
    }
}