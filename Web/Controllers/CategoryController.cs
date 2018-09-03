using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.App_Start;
using Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Common;
using Web.Server;
using RazorBase;

namespace Web.Controllers
{
    /// <summary>
    /// 栏目控制器
    /// </summary>
    public class CategoryController : BaseController
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Edit(int ID)
        {
            return View(mangae.db.TG_Category.Find(ID));
        }

        /// <summary>
        /// 获取栏目数据列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var query = mangae.db.TG_Category.OrderBy(m => m.iOrder).AsQueryable();
            var main = query.Where(m => m.CategoryId == 0).ToList();
            var child = query.Where(m => m.CategoryId > 0).ToList();
            JArray comtreeData = new JArray();
            foreach (var m in main)
            {
                JObject mainData = JsonHelper.Object(JsonHelper.toJson(m));
                var array = recursion(m, child,1);
                mainData.Add(new JProperty("children", array));
                comtreeData.Add(mainData);
            }
            return Content(comtreeData.ToString());
        }

        /// <summary>
        /// 获取栏目ComtreeData
        /// </summary>
        /// <returns></returns>
        public ActionResult CategoryComtreeData(int? IsGen)
        {
           
            var query = mangae.db.TG_Category.OrderBy(m => m.iOrder).AsQueryable();
            var main = query.Where(m => m.CategoryId == 0).ToList();
            var child = query.Where(m => m.CategoryId > 0).ToList();

            JArray comtreeData = new JArray();
            if (IsGen == null)
            {
                var data = new JObject();
                data.Add(new JProperty("id", 0));
                data.Add(new JProperty("text", "根栏目"));
                comtreeData.Add(data);
            }
          
               
            foreach (var m in main)
            {
                JObject mainData = new JObject();
                mainData.Add(new JProperty("id", m.ID));
                mainData.Add(new JProperty("text", m.sName));
                var array = recursion(m, child);
                mainData.Add(new JProperty("children", array));
                comtreeData.Add(mainData);
            }
            return Content(comtreeData.ToString());
        }

        /// <summary>
        /// 递归调用
        /// </summary>
        /// <returns></returns>
        public JArray recursion(TG_Category category, List<TG_Category> list,int type=0)
        {
            JArray mainArray = new JArray();
            if (type == 1)
            {
                foreach (var n in list)
                {
                    if (category.ID == n.CategoryId)
                    {
                        JObject childData = JsonHelper.Object(JsonHelper.toJson(n));
                        //函数递归调用
                        var array = recursion(n, list,1);
                        childData.Add(new JProperty("children", array));
                        mainArray.Add(childData);
                    }
                }
            }
            else
            {
                foreach (var n in list)
                {
                    if (category.ID == n.CategoryId)
                    {
                        JObject childData = new JObject();
                        childData.Add(new JProperty("id", n.ID));
                        childData.Add(new JProperty("text", n.sName));
                        //函数递归调用
                        var array = recursion(n, list);
                        childData.Add(new JProperty("children", array));
                        mainArray.Add(childData);
                    }
                }
            }
            return mainArray;
        }


        /// <summary>
        /// 添加栏目
        /// </summary>
        /// <param name="category"></param>
        [ValidateInput(false)]
        public void Insert(TG_Category category)
        {
            if(mangae.db.TG_Category.Any(m=>m.sName== category.sName || m.sEnName == category.sEnName))
            {//存在相同的栏目名称或栏目标识
                result.info = "存在相同的栏目名称或栏目标识";
              return;
            }
            mangae.Add<TG_Category>(category);
            result.success = mangae.SaveChange();
        }

        /// <summary>
        /// 编辑栏目
        /// </summary>
        /// <param name="category"></param>
        [ValidateInput(false)]
        public void Update(TG_Category category)
        {
            if (mangae.db.TG_Category.Where(m=>m.ID!= category.ID).Any(m => m.sName == category.sName || m.sEnName == category.sEnName))
            {//存在相同的栏目名称或栏目标识
                result.info = "存在相同的栏目名称或栏目标识";
                return;
            }
            mangae.Edit<TG_Category>(category);
            result.success = mangae.SaveChange();

            /**编辑栏目重新生成栏目页***/
            var templet = mangae.db.TG_Templet.Where(m => m.ID == category.iTemplateId).SingleOrDefault();
            if (templet != null)
            {//模板存在
                string templetHtmlString = RazorHelper.ParseString(templet.sTempletEnName, category);
                string sHtmlPath = FuncHelper.Instance.GetHtmlPath(category, mangae.db.TG_Category.ToList());
                RazorHelper.MakeHtml(sHtmlPath, category.sEnName, templetHtmlString);
            }
        }

        /// <summary>
        /// 删除栏目
        /// </summary>
        /// <param name="Ids"></param>
        public void Cancel(string Ids)
        {
            result.success = mangae.Delete<TG_Category>(Ids);
        }


    }
}