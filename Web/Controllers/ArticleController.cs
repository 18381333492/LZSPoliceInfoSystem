using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.App_Start;
using Web.Models;

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
            var config=mangae.db.TG_WebSite.SingleOrDefault();
            article.bIsRelease = config.IsNeedVerify;
            article.bIsDeleted = false;
            article.dInsertTime = DateTime.Now;
            mangae.Add<TG_Article>(article);
            result.success = mangae.SaveChange();
        }


        /// <summary>
        /// 编辑文章
        /// </summary>
        /// <param name="article"></param>
        [ValidateInput(false)]
        public void Update(TG_Article article)
        {
            mangae.Edit<TG_Article>(article);
            result.success = mangae.SaveChange();
        }


        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="Ids"></param>
        public void Cancel(string Ids)
        {
           var res=mangae.Cancel<TG_Article>(Ids);
            if (res > 0)
                result.success = true;
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
    }
}