using RazorBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.App_Start;
using Web.Models;
using Web.Server;

namespace Web.Controllers
{

    /// <summary>
    /// 生成页面的控制器
    /// </summary>
    public class MakeHtmlController : BaseController
    {
        // GET: MakeHtml
        public ActionResult Index()
        {
            return View();
        }

  
        /// <summary>
        /// 生成栏目页
        /// </summary>
        /// <param name="Ids"></param>
        public void MakeCategoryPage(string Ids)
        {
            int iSuccessCount = 0;//成功条数
            int iFalseCount = 0;//失败条数

            var allCategoryList = mangae.db.TG_Category.OrderBy(m => m.ID).AsQueryable().ToList();//所有栏目数据
            var allTempletList = mangae.db.TG_Templet.OrderBy(m => m.ID).AsQueryable().ToList();//所有模板数据
            var listIds = Ids.Split(',').Select(m => Convert.ToInt32(m)).ToList();
            var CategoryList = allCategoryList.Where(m => listIds.Contains(m.ID)).ToList();//要生成栏目数据

            //循环生成栏目页
            foreach (var category in CategoryList)
            {
                var templet = allTempletList.Where(m => m.ID == category.iTemplateId).SingleOrDefault();
                if (templet != null)
                {//模板存在
                   
                    string templetHtmlString = RazorHelper.ParseString(templet.sTempletEnName, category);
                    string sHtmlPath = FuncHelper.Instance.GetHtmlPath(category, allCategoryList);
                    if (RazorHelper.MakeHtml(sHtmlPath, category.sEnName, templetHtmlString))
                    {
                        iSuccessCount++;
                    }
                    else
                    {
                        iFalseCount++;
                    }
                }
            }
            result.success = true;
            result.data = new
            {
                iSuccessCount = iSuccessCount,
                iFalseCount = iFalseCount,
                iTatolCount = iSuccessCount + iFalseCount
            };
        }


        /// <summary>
        /// 生成文章页
        /// </summary>
        /// <param name="Ids"></param>
        public void MakeArticlePage(string Ids)
        {
            int iSuccessCount = 0;//成功条数
            int iFalseCount = 0;//失败条数

            var allCategoryList = mangae.db.TG_Category.OrderBy(m => m.ID).AsQueryable().ToList();//所有栏目数据
            var allTempletList = mangae.db.TG_Templet.OrderBy(m => m.ID).AsQueryable().ToList();//所有的模板数据
            var listIds = Ids.Split(',').Select(m => Convert.ToInt32(m)).ToList();
            var CategoryList = allCategoryList.Where(m => listIds.Contains(m.ID)).ToList();//获取生成栏目数据

            //获取栏目的ID集合
            var CategoryIds = CategoryList.Select(m => m.ID);
            //获取生成栏目下的所有文章
            List<TG_Article> ArticleList = mangae.db.TG_Article.Where(m => CategoryIds.Contains(m.iCategoryId)&&m.bIsDeleted==false).ToList();
            //循环生成文章页
            foreach (var category in CategoryList)
            {
                //获取栏目下的文章模板
                TG_Templet templetArticle = allTempletList.Where(m => m.ID == category.iArticleTemplateId).SingleOrDefault();
                //获取当前栏目下的文章数据列表
                var CurrentArticleList = ArticleList.Where(m => m.iCategoryId == category.ID).ToList();
                //遍历当前栏目下的文章
                foreach (var article in CurrentArticleList)
                {
                    if (article.iTemplateId != 0)
                    {//获取文章是否有独立模板
                     //有则使用独立模板
                        templetArticle = allTempletList.FirstOrDefault(m => m.ID == article.iTemplateId);
                    }
                    if (templetArticle != null)
                    {//模板存在
                     //解析模板        
                        string templetHtmlString = RazorHelper.ParseString(templetArticle.sTempletEnName, article);
                        string ArticlePath = FuncHelper.Instance.GetHtmlPath(category, allCategoryList);
                        if (RazorHelper.MakeHtml(ArticlePath, article.ID.ToString(), templetHtmlString))
                        {//生成成功
                            iSuccessCount++;
                        }
                        else
                        {//生成失败
                            iFalseCount++;
                        }
                    }
                }
            }
            result.success = true;
            result.data = new
            {
                iSuccessCount = iSuccessCount,
                iFalseCount = iFalseCount,
                iTatolCount = iSuccessCount + iFalseCount
            };
        }
    }
}