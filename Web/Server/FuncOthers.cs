using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;

namespace Web.Server
{
    public partial class FuncHelper
    {
        private static readonly string sHtmlBasePath = ConfigHelper.ReadAppSetting("sHtmlPath");

        /// <summary>
        /// 根据栏目获取栏目生成文件的路径
        /// </summary>
        /// <returns></returns>
        public string GetHtmlPath(TG_Category category, List<TG_Category> categoryList)
        {
            string sPath = string.Empty;
            if (category.CategoryId != 0)
            {//子级栏目
                TG_Category parentCategory = null;
                do
                {
                    parentCategory = categoryList.FirstOrDefault(m => m.ID == category.CategoryId);
                    sPath = sPath + "\\" + parentCategory.sEnName.ToLower();
                }
                while (parentCategory != null && parentCategory.CategoryId != 0);
                sPath = sPath.TrimStart('\\');
            }
            return sPath;
        }


        /// <summary>
        /// 根据获取文章的链接
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public string GetArticleHtmlUrl(int articleId)
        {
            using (var db = new Entities())
            {
                string sUrl = string.Empty;
                var article = db.TG_Article.Find(articleId);
                var category = db.TG_Category.Find(article.iCategoryId);
                var categoryList = db.TG_Category.ToList();
                if (category.CategoryId != 0)
                {
                    sUrl = category.sEnName.ToLower();
                    TG_Category parentCategory = null;
                    do
                    {
                        parentCategory = categoryList.FirstOrDefault(m => m.ID == category.CategoryId);
                        sUrl = parentCategory.sEnName.ToLower() + "/" + sUrl;
                    }
                    while (parentCategory != null && parentCategory.CategoryId != 0);
                    sUrl = sHtmlBasePath + "/" + sUrl;
                }
                else
                {
                    sUrl = string.Format("/{0}/{1}", sHtmlBasePath, category.sEnName.ToLower());
                }
                sUrl = string.Format("{0}/{1}.html", sUrl, articleId);
                return sUrl;
            }
        }

        /// <summary>
        /// 根据栏目ID和分页数获取栏目网络连接
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public string GetCategoryHtmlUrl(int categoryId,int page)
        {
            using (var db = new Entities())
            {
                string sUrl = string.Empty;
                var category = db.TG_Category.Find(categoryId);
                var categoryList = db.TG_Category.ToList();
                if (category.CategoryId != 0)
                {
                    sUrl = category.sEnName.ToLower();
                    TG_Category parentCategory = null;
                    do
                    {
                        parentCategory = categoryList.FirstOrDefault(m => m.ID == category.CategoryId);
                        sUrl = parentCategory.sEnName.ToLower() + "/" + sUrl;
                    }
                    while (parentCategory != null && parentCategory.CategoryId != 0);
                    sUrl = string.Format("/{0}/{1}_{2}.html",sHtmlBasePath, sUrl, page);
                }
                else
                {
                    sUrl = string.Format("/{0}/{1}_{2}.html", sHtmlBasePath, category.sEnName.ToLower(), page);
                }
                return sUrl;
            }
        }          

        /// <summary>
        /// 根据栏目ID获取栏目网络连接
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>

        public string GetCategoryHtmlUrl(int categoryId)
        {
            using (var db = new Entities())
            {
                string sUrl = string.Empty;
                var category = db.TG_Category.Find(categoryId);
                var categoryList = db.TG_Category.ToList();
                if (category.CategoryId != 0)
                {
                    sUrl = category.sEnName.ToLower();
                    TG_Category parentCategory = null;
                    do
                    {
                        parentCategory = categoryList.FirstOrDefault(m => m.ID == category.CategoryId);
                        sUrl = parentCategory.sEnName.ToLower() + "/" + sUrl;
                    }
                    while (parentCategory != null && parentCategory.CategoryId != 0);
                    sUrl = sHtmlBasePath + "/" + sUrl + ".html";
                }
                else
                {
                    sUrl = string.Format("/{0}/{1}.html", sHtmlBasePath, category.sEnName.ToLower());
                }
                return sUrl;
            }
        }

                
        /// <summary>
        /// 获取栏目位置
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public List<string> GetCategoryLocation(int categoryId)
        {
            using (var db = new Entities())
            {
                List<string> LocationArray = new List<string>();
                string sUrl = string.Empty;
                var category = db.TG_Category.Find(categoryId);
                var categoryList = db.TG_Category.ToList();
                if (category.CategoryId != 0)
                {
                    TG_Category parentCategory = null;
                    do
                    {
                        parentCategory = categoryList.FirstOrDefault(m => m.ID == category.CategoryId);
                        LocationArray.Add(parentCategory.sName);
                    }
                    while (parentCategory != null && parentCategory.CategoryId != 0);
                }
                else
                {
                    LocationArray.Add(category.sName);
                }
                return LocationArray;
            }
        }



    }
}