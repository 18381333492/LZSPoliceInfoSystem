using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorBase
{

    /// <summary>
    /// 自定义模板的方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RazorFunc<T> : TemplateBase<T>
    {
        /// <summary>
        /// 初始化构造函数
        /// 实现封装的一些公用方法 (第二种表现形式) 调用的时候直接@Helper.方法
        /// </summary>
        //public RazorFunc()
        //{
        //    Helper = new MyCommon();
        //}
        //public static MyCommon Helper { get; set; }
    }


    /// <summary>
    /// 通用方法的封装(第一种表现形式)
    /// </summary>
    public class MyCommon
    {
        public static MyCommon Helper =new MyCommon();
        public string Upper()
        {
            return "1141654564654564";
        }

    }
}
