using RazorBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;

namespace Web.Server
{
    /// <summary>
    /// 通用公共函数
    /// </summary>
    public partial class FuncHelper
    {
        
        private static FuncHelper instance=null;

        /// <summary>
        /// 对象实例
        /// </summary>
        public static FuncHelper Instance
        {
            get
            {
                if (instance == null)
                    instance = new FuncHelper();
                return instance;
            }
        }
        /// <summary>
        /// 获取站点信息
        /// </summary>
        public TG_WebSite WebSite
        {
            get
            {
                using (var db = new Entities())
                {
                    var web = db.TG_WebSite.FirstOrDefault();
                    return web;
                }
            }
        }
        //********************************************************常用的方法的封装************************************************************//

    
        public TG_WebSite GetConfig()
        {
            using (var db = new Entities())
            {
                var web = db.TG_WebSite.FirstOrDefault();
                return web;
            }
        }

        /// <summary>
        /// 根据主键ID获取实体对象
        /// </summary>
        /// <returns></returns>
        public T GetModel<T>(int ID) where T: class, new()
        {
            using (var db = new Entities())
            {
                T item = db.Set<T>().Find(ID);
                return item;
            }
        }

        /// <summary>
        /// 获取实体所有的数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetModelList<T>() where T : class, new()
        {
            using (var db = new Entities())
            {
                List<T> list = db.Set<T>().ToList();
                return list;
            }
        }


        /// <summary>
        /// 获取所有显示的栏目
        /// </summary>
        public List<TG_Category> GetShowCategory()
        {
            using (var db = new Entities())
            {
                var list = db.TG_Category.Where(m => m.bIsShowNav == true && m.CategoryId == 0)
                                          .OrderBy(m => m.iOrder).ToList();
                return list;
            }
        }

        /// <summary>
        /// 根据一级栏目ID获取下面的子栏目
        /// </summary>
        /// <param name="iParentCategoryId"></param>
        /// <returns></returns>
        public List<TG_Category> GetChildCategory(int iParentCategoryId)
        {
            using (var db = new Entities())
            {
                var list = db.TG_Category.Where(m => m.bIsShowNav == true && m.CategoryId == iParentCategoryId)
                                          .OrderBy(m => m.iOrder).ToList();
                return list;
            }
        }

        /// <summary>
        /// 根据一级栏目ID获取下面的子栏目
        /// </summary>
        /// <param name="iParentCategoryId"></param>
        /// <returns></returns>
        public List<TG_Category> GetChildCategory(int iParentCategoryId,bool bIsShowNav)
        {
            using (var db = new Entities())
            {
                var list = db.TG_Category.Where(m => m.bIsShowNav == bIsShowNav && m.CategoryId == iParentCategoryId)
                                          .OrderBy(m => m.iOrder).ToList();
                return list;
            }
        }

        /// <summary>
        /// 获取栏目下面的所有文章
        /// </summary>
        /// <param name="iParentCategoryId"></param>
        /// <returns></returns>
        public List<TG_Article> GetCategoryAllArticleList(int iParentCategoryId)
        {
            using (var db = new Entities())
            {
                var list = db.TG_Category.Where(m => m.bIsShowNav ==false && m.CategoryId == iParentCategoryId)
                                          .OrderBy(m => m.iOrder).Select(m=>m.ID).ToList();
                var AllArticleList = db.TG_Article.Where(m => list.Contains(m.iCategoryId)).ToList();
                return AllArticleList;
            }
        }


        /// <summary>
        /// 根据栏目的英文标识获取栏目
        /// </summary>
        /// <param name="sEnName"></param>
        /// <returns></returns>
        public TG_Category GetCategoryByEnName(string sEnName)
        {
            using (var db = new Entities())
            {
                var category = db.TG_Category.FirstOrDefault(m => m.sEnName == sEnName);
                return category;
            }
        }

        /// <summary>
        /// 根据栏目主键ID获取栏目下的文章列表
        /// </summary>
        /// <param name="iCategoryId"></param>
        /// <returns></returns>
        public List<TG_Article> GetArticleListByCategoryId(int iCategoryId)
        {
            using (var db = new Entities())
            {
                var ArticleList = db.TG_Article.Where(m => m.iCategoryId == iCategoryId&&m.bIsDeleted==false).OrderBy(m=>m.sSize).ToList();
                return ArticleList;
            }
        
        }
    }
}