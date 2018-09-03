using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public enum OrderType
    {
        ASC = 1,
        DESC = 2,
    }
    /// <summary>
    /// 分页数据对象
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        /// 当前页索引
        /// </summary>
        public int page
        {
            get;
            set;
        } = 1;

        /// <summary>
        /// 页面数据的大小
        /// </summary>
        public int rows
        {
            get;
            set;
        } = 10;

        /// <summary>
        /// 排序的字段名称
        /// </summary>
        public string sort
        {
            get;
            set;
        }

        /// <summary>
        /// 排序方式asc or desc
        /// </summary>
        public OrderType order
        {
            get;
            set;
        } = OrderType.DESC;

        /// <summary>
        /// 关键字
        /// </summary>
        public string sKeyWord
        {
            get;
            set;
        }
    }
}