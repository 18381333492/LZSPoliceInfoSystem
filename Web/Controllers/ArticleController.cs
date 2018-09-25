using RazorBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.App_Start;
using Web.Models;
using Web.Server;

namespace Web.Controllers
{
    /// <summary>
    /// 文章控制器
    /// </summary>
    public class ArticleController : BaseController
    {
        // GET: Article
        public ActionResult Index()
        {
            ViewBag.bIsSuper = LoginStatus.bIsSuper;
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Edit(int ID)
        {
            return View(mangae.db.TG_Article.Find(ID));
        }

        /// <summary>
        /// 分页获取文章数据列表
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <param name="iCategoryId"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public ActionResult List(PageInfo pageInfo,int iCategoryId=0)
        {
            var query = from m in mangae.db.TG_Article
                        join n in mangae.db.TG_Category
                        on m.iCategoryId equals n.ID
                        where m.bIsDeleted == false
                        orderby m.ID descending
                        select new
                        {
                            m.ID,
                            m.iCategoryId,
                            n.sName,//栏目名称
                            m.sTitle,
                            m.sAuthor,
                            m.bIsHot,
                            m.bIsTop,
                            m.bIsSlide,
                            m.bIsRelease,
                            m.dInsertTime

                        };
            if (iCategoryId > 0)
                query = query.Where(m => m.iCategoryId == iCategoryId);
            if (!string.IsNullOrEmpty(pageInfo.sKeyWord))
                query = query.Where(m => m.sTitle.Contains(pageInfo.sKeyWord));
            result.pageResult.total = query.Count();
            query = query.Skip((pageInfo.page - 1) * pageInfo.rows).Take(pageInfo.rows);
            result.pageResult.rows = query;
            return Content(result.pageResult.toJson());
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="article"></param>
        [ValidateInput(false)]
        public void Insert(TG_Article article)
        {
            if (LoginStatus.iUserType == 0)
            {
                if (string.IsNullOrEmpty(LoginStatus.sCategoryIds)||LoginStatus.sCategoryIds.Contains(article.iCategoryId.ToString()) == false)
                {
                    result.info = "您没有权限操作";
                }
                else
                {
                    article.bIsRelease = true;
                    article.bIsDeleted = false;
                    article.dInsertTime = DateTime.Now;
                    mangae.Add<TG_Article>(article);
                    result.success = mangae.SaveChange();
                    if (result.success)
                    {
                        MakeArticleHtml(article);
                    }
                }
            }
            else
            {
                article.bIsRelease = true;
                article.bIsDeleted = false;
                article.dInsertTime = DateTime.Now;
                mangae.Add<TG_Article>(article);
                result.success = mangae.SaveChange();
                if (result.success)
                {
                    MakeArticleHtml(article);
                }
            }
        }

        /// <summary>
        /// 编辑文章
        /// </summary>
        /// <param name="article"></param>
        [ValidateInput(false)]
        public void Update(TG_Article article)
        {
            if (LoginStatus.iUserType == 0)
            {
                if (string.IsNullOrEmpty(LoginStatus.sCategoryIds) || LoginStatus.sCategoryIds.Contains(article.iCategoryId.ToString()) == false)
                {
                    result.info = "您没有权限操作";
                }
                else
                {
                    article.bIsRelease = true;
                    mangae.Edit<TG_Article>(article);
                    result.success = mangae.SaveChange();
                    if (result.success)
                    {
                        MakeArticleHtml(article);
                    }
                }
            }
            else
            {
                article.bIsRelease = true;
                mangae.Edit<TG_Article>(article);
                result.success = mangae.SaveChange();
                if (result.success)
                {
                    MakeArticleHtml(article);
                }
            }
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="Ids"></param>
        public void Cancel(string Ids)
        {
            var article = mangae.db.TG_Article.Find(Ids);
            if (LoginStatus.iUserType == 0)
            {
                if (string.IsNullOrEmpty(LoginStatus.sCategoryIds) || LoginStatus.sCategoryIds.Contains(article.iCategoryId.ToString()) == false)
                {
                    result.info = "您没有权限操作";
                }
                else
                {
                    var res = mangae.Delete<TG_Article>(Ids);
                    if (res)
                    {
                        result.success = true;
                        MakeArticleHtml(article);
                    }
                }
            }
            else
            {
                var res = mangae.Delete<TG_Article>(Ids);
                if (res)
                {
                    result.success = true;
                    MakeArticleHtml(article);
                }

            }
        }

        /// <summary>
        /// 审核文章
        /// </summary>
        /// <param name="Ids"></param>
        public void Verify(string Ids)
        {
            var res=mangae.ExcuteBySql(string.Format("update TG_Article set bIsRelease=1 where ID IN ({0})", Ids));
            if (res > 0)
                result.success = true;
        }

        public void MakeArticleHtml(TG_Article article)
        {
            try
            {
                //获取文章所属栏目
                var category = mangae.db.TG_Category.Find(article.iCategoryId);
                var allCategoryList = mangae.db.TG_Category.OrderBy(m => m.ID).AsQueryable().ToList();
                TG_Templet templetArticle;//生成文章的模板
                if (article.iTemplateId != 0)
                {//存在独立文章模板
                    templetArticle = mangae.db.TG_Templet.Find(article.iTemplateId);
                }
                else
                {
                    //获取栏目的文章模板ID
                    templetArticle = mangae.db.TG_Templet.Find(category.iArticleTemplateId);
                }
                if (templetArticle != null)
                {
                    string templetHtmlString = RazorHelper.ParseString(templetArticle.sTempletEnName, article);
                    string ArticlePath = category.sEnName + FuncHelper.Instance.GetHtmlPath(category, allCategoryList);
                    RazorHelper.MakeHtml(ArticlePath, article.ID.ToString(), templetHtmlString);
                }

                //生成文章页栏目
                if (category.bIsContentCategory==null&& category.bIsRedirect==null)
                {//文章栏目
                    var category_templet = mangae.db.TG_Templet.Where(m => m.ID == category.iTemplateId).SingleOrDefault();
                    //文章分页栏目
                    //获取栏目下面所有文章数量
                    var articleTotalCount = mangae.db.TG_Article.
                        Where(m => m.iCategoryId == category.ID && m.bIsDeleted == false && m.bIsRelease == true).Count();
                    int categoryHtmlCount = (int)Math.Ceiling((double)articleTotalCount / (double)20); //生成栏目页的个数
                    var pageCategory = new PageCategory();
                    pageCategory.Category = category;
                    pageCategory.Rows = categoryHtmlCount;//总页数
                    for (var i = 1; i <= categoryHtmlCount; i++)
                    {
                        pageCategory.Page = i;
                        string templetHtmlString = RazorHelper.ParseString(category_templet.sTempletEnName, pageCategory);
                        if (!string.IsNullOrEmpty(templetHtmlString))
                        {
                            string sHtmlPath = FuncHelper.Instance.GetHtmlPath(category, allCategoryList);
                            string sFileName = string.Format("{0}_{1}", category.sEnName, i);
                            RazorHelper.MakeHtml(sHtmlPath, sFileName, templetHtmlString)   ;               
                        }
                    }
                }

                //生成首页          
                var index_category = FuncHelper.Instance.GetCategoryByEnName("index");
                var index_templet = mangae.db.TG_Templet.Where(m => m.ID == index_category.iTemplateId).SingleOrDefault();
                if (index_templet != null)
                {//模板存在
                    string templetHtmlString = RazorHelper.ParseString(index_templet.sTempletEnName, index_category);
                    string sHtmlPath = FuncHelper.Instance.GetHtmlPath(index_category, allCategoryList);
                    RazorHelper.MakeHtml(sHtmlPath, index_category.sEnName, templetHtmlString);
                }
            }
           finally
            {
               
            }
           
        }
    }
}